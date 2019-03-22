using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.RealTime;
using Abp.Threading;
using Abp.Linq.Extensions;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using Abp.SignalR.Core;
using Abp.SignalR.Core.Hubs;
using Microsoft.AspNetCore.SignalR;
using Abp.Extensions;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Application;
using Abp.WorkFlow;

namespace HR
{
    public class ProjectNoticeAppService : FRMSCoreAppServiceBase, IProjectNoticeAppService
    {
        private readonly ProjectNoticeManager _noticeManager;
        private readonly ISignalrNoticeAppService _noticeCommunicator;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly IRepository<NoticeTexts, Guid> _noticeTextRepository;
        private readonly IRepository<NoticeLogs, Guid> _noticeLogRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<Follow, Guid> _followRepository;
        public ProjectNoticeAppService(ProjectNoticeManager noticeManager,
            ISignalrNoticeAppService noticeCommunicator,
            IOnlineClientManager onlineClientManager,
            IRepository<NoticeTexts, Guid> noticeTextRepository, IRepository<NoticeLogs, Guid> noticeLogRepository, IRepository<User, long> useRepository,
             IServiceProvider service, IWorkFlowTaskRepository workFlowTaskRepository, IRepository<Follow, Guid> followRepository)
        {
            _noticeManager = noticeManager;
            _noticeCommunicator = noticeCommunicator;
            _onlineClientManager = onlineClientManager;
            _noticeTextRepository = noticeTextRepository;
            _noticeLogRepository = noticeLogRepository;
            _userRepository = useRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _followRepository = followRepository;
        }
         

        /// <summary>
        /// 发布通知公告 和 发布公司新闻
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateNotice(NoticePublishInput input)
        {
            var userids = new List<long>();
            if (string.IsNullOrWhiteSpace(input.NoticeUserIds) == false)
            {
                userids.AddRange(Array.ConvertAll(input.NoticeUserIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), new Converter<string, long>(ite => long.Parse(ite))));
            }
            if (string.IsNullOrWhiteSpace(input.NoticeGroupIds) == false)
            {
                var groupids = Array.ConvertAll(input.NoticeGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), new Converter<string, Guid>(ite => Guid.Parse(ite)));
                //var tmpuserids = _projectAuditGroupUserRepository.GetAll().Where(ite => groupids.Contains(ite.GroupId)).Select(ite => ite.UserId).ToList();
                //userids.AddRange(tmpuserids);
            }
            if (string.IsNullOrWhiteSpace(input.NoticeDepartmentIds) == false)
            {
                var govids = Array.ConvertAll(input.NoticeDepartmentIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), new Converter<string, Guid>(ite => Guid.Parse(ite)));
                foreach (var g in govids)
                {
                    int count = 0;
                    //var users = _projectBaseRepository.GetUsersWithCurrentAndUnderOrg(g, 100, 1, out count).Select(ite => ite.Id);
                    //userids.AddRange(users);
                }
            }
            if (userids.Count > 0)
            {
                userids = userids.Distinct().ToList();
                //var onlineClients = _onlineClientManager.GetAllClients().Where(p => p.UserId.HasValue && userids.Contains(p.UserId.Value)).ToList();
                //_noticeCommunicator.SendNoticeToClient(onlineClients, null, input.Title, input.Content, link: "Mpa/DailyOfficeNotice");

            }
            if (input.Id.HasValue)
            {
                await UpdateNoticeAsync(input);
            }
            else
            {
                await CreateNotice(input, userids);
            }
        }



        protected async Task CreateNotice(NoticePublishInput input, List<long> users = null)
        {
            if (input.Id.HasValue)
            {

            }
            var userId = AbpSession.GetUserId();

            var textId = await _noticeTextRepository.InsertAndGetIdAsync(new NoticeTexts
            {
                CreationTime = DateTime.Now,
                ExpireTime = new DateTime(2099, 09, 09),
                MsgConent = input.Content,
                NoticeType = input.NoticeType,
                ProjectId = null,
                Title = input.Title,
                SendUserId = userId,
                NoticeDepartmentIds = input.NoticeDepartmentIds,
                NoticeGroupIds = input.NoticeGroupIds,
                NoticeUserIds = input.NoticeUserIds,
                NoticeAllUserIds = users == null ? null : string.Join(",", users)
            });

            if (input.NoticeType == (int)SystemNoticeType.通知公告 && users.Count == 0)
                input.IsAllRecive = true;

            if (input.NoticeType == (int)SystemNoticeType.公司新闻)
            {
                await _noticeLogRepository.InsertAsync(new NoticeLogs
                {
                    TextId = textId,
                    ReceiveId = userId,
                    Status = 1,
                    NoticeType = input.NoticeType,
                    CreationTime = DateTime.Now
                });
            }
            else if (input.NoticeType == (int)SystemNoticeType.通知公告)
            {
                if (input.IsAllRecive)
                {
                    await _noticeLogRepository.InsertAsync(new NoticeLogs
                    {
                        TextId = textId,
                        ReceiveId = userId,
                        Status = 1,
                        NoticeType = input.NoticeType,
                        CreationTime = DateTime.Now
                    });
                }
                else
                {
                    if (users.Count > 0)
                    {
                        foreach (var item in users)
                        {
                            await _noticeLogRepository.InsertAsync(new NoticeLogs
                            {
                                TextId = textId,
                                ReceiveId = item,
                                Status = 1,
                                NoticeType = input.NoticeType,
                                CreationTime = DateTime.Now
                            });
                        }
                    }

                }

            }
            else
            {

            }


            //日志功能暂时屏蔽
            //var logservice = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Logapp.ILogAppService>();
            //await logservice.CreateOrUpdateLogV2("发布公告", string.Format("发布公告，标题：{0}", input.Title), SystemLogTypes.日常办公.ToString());
            await CurrentUnitOfWork.SaveChangesAsync();


            var onlineClients = new List<IOnlineClient>();
            if (input.NoticeType == (int)SystemNoticeType.通知公告 && !input.IsAllRecive)
            {
                onlineClients = _onlineClientManager.GetAllClients().Where(p => p.UserId != userId && users.Contains(p.UserId.Value)).ToList();
            }
            else
            {
                onlineClients = _onlineClientManager.GetAllClients().Where(p => p.UserId != userId).ToList();
            }

            _noticeCommunicator.SendNoticeToClient(onlineClients, null, input.Title, string.Empty);
        }

        protected async Task UpdateNoticeAsync(NoticePublishInput input)
        {
            var noticemodel = await _noticeTextRepository.GetAll().FirstOrDefaultAsync(r => r.Id == input.Id);
            if (noticemodel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
            else
            {
                noticemodel.Title = input.Title;
                noticemodel.MsgConent = input.Content;
                noticemodel.NoticeDepartmentIds = input.NoticeDepartmentIds;
                noticemodel.NoticeGroupIds = input.NoticeGroupIds;
                noticemodel.NoticeUserIds = input.NoticeUserIds;
                await _noticeTextRepository.UpdateAsync(noticemodel);
                //var logservice = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Logapp.ILogAppService>();
                //await logservice.CreateOrUpdateLogV2("更新公告", string.Format("更新公告，标题：{0}", input.Title), SystemLogTypes.日常办公.ToString());
            }

        }


        public async Task SendAsync(NoticeMessageInput input)
        {
            if (!input.ReceiveId.HasValue && !input.RoadFlowReceiveId.HasValue)
                return;

            long receiveId = 0;
            receiveId = input.ReceiveId.Value;
            if (input.OnlyTip.HasValue && !input.OnlyTip.Value)
            {
                await _noticeManager.SendPrivateMessageAsync(receiveId, input.ProjectId, input.Title, input.Content, input.Link);
            }
            var userId = AbpSession.GetUserId();

            var userIdentifier = AbpSession.ToUserIdentifier();
            var onlineClients = _onlineClientManager.GetAllClients().Where(p => p.UserId == receiveId).ToList();

            //_noticeCommunicator.SendNoticeToClient(onlineClients, input.ProjectId, input.Title, input.Content, input.Link);

        }

        public void Send(NoticeMessageInput input)
        {
            AsyncHelper.RunSync(() => SendAsync(input));
        }

        private IQueryable<NoticeList> CreateNoticeQuery(int? type)
        {
            if (AbpSession.UserId.HasValue == false)
                return null;

            return (from log in _noticeLogRepository.GetAll()
                    join texts in _noticeTextRepository.GetAll() on log.TextId equals texts.Id into notice
                    from text in notice.DefaultIfEmpty()
                    join user in _userRepository.GetAll() on text.CreatorUserId equals user.Id
                    where log.ReceiveId == AbpSession.UserId.Value && log.Status != 3 && (type.HasValue && type.Value != 0 ? text.NoticeType == type : 1 == 1)
                    orderby log.CreationTime descending
                    select new NoticeList
                    {
                        LogId = log.Id,
                        CreateUserId = log.CreatorUserId.Value,
                        Content = text.MsgConent,
                        CreationTime = text.CreationTime,
                        CreateUserName = user == null ? "" : user.Name,
                        IsRead = log.Status != 1,
                        TextId = text.Id,
                        Title = text.Title,
                        Type = log.NoticeType
                    });
        }


        /// <summary>
        /// 获取 消息中心的 通知公告 公司新闻 事务通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<NoticeList>> GetPage(GetNoticeListInput input)
        {
            var query = CreateNoticeQuery(input.NoticeType);
            if (query == null)
                return new PagedResultDto<NoticeList>(0, new List<NoticeList>());
            query = query.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), r => r.Title.Contains(input.SearchKey) || r.CreateUserName.Contains(input.SearchKey));
            query =
                    (from a in query
                     join b in _followRepository.GetAll().Where(x => x.BusinessType == FollowType.公告 && x.CreatorUserId == AbpSession.UserId.Value) on a.LogId equals b.BusinessId into tmp1
                     from c in tmp1.DefaultIfEmpty()
                     where (input.IsFollow ? c != null : true)
                     select new NoticeList()
                     {
                         LogId = a.LogId,
                         TextId = a.TextId,
                         Title = a.Title,
                         Content = a.Content,
                         IsRead = a.IsRead,
                         ProjectId = a.ProjectId,
                         Type = a.Type,
                         CreateUserName = a.CreateUserName,
                         CreateUserId = a.CreateUserId,
                         IsFollow = c != null,
                         CreationTime = a.CreationTime
                     }
            );

            var total = await query.CountAsync();
            var list = await query.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

            if (input.NoticeType == 1)
            {
                foreach (var m in list)
                {
                    if (m.IsRead)
                        continue;
                    var log = await _noticeLogRepository.GetAsync(m.LogId);
                    if (log != null)
                    {
                        log.Status = 2;
                        log.ReadTime = DateTime.Now;
                        await _noticeLogRepository.UpdateAsync(log);
                    }
                }
            }
          
            return new PagedResultDto<NoticeList>(total, list);
        }

        /// <summary>
        /// type: 1 事务通知 2 通知公告 3 公司新闻
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<ListResultDto<NoticeList>> GetNotReadList(int? type)
        {
            if (AbpSession.UserId.HasValue)
            {
                //未收到的公告
                var userId = AbpSession.UserId.Value;
                var textIds = GetNotReceiveText(userId);
                if (textIds.Count > 0)
                {
                    foreach (var m in textIds)
                    {
                        //以后优化
                        if (_noticeLogRepository.GetAll().Where(p => p.ReceiveId == userId && p.TextId == m.Id).Count() > 1)
                            continue;
                        _noticeLogRepository.Insert(new NoticeLogs
                        {
                            TextId = m.Id,
                            ReceiveId = userId,
                            Status = 1,
                            NoticeType = m.NoticeType,
                            CreationTime = DateTime.Now
                        });
                    }
                    CurrentUnitOfWork.SaveChanges();
                }
            }
            var query = CreateNoticeQuery(type).Where(p => p.IsRead == false);
            var list = await query.ToListAsync();

            return new ListResultDto<NoticeList>(list);
        }

        private List<NoticeNoRead> GetNotReceiveText(long userId)
        {
            //我现有的已收到的log
            var logQuery = (from log in _noticeLogRepository.GetAll()
                            where log.ReceiveId == userId && log.NoticeType >= 2
                            select log.TextId).ToList();

            //我没收到的未过期的公告
            var textQuery = (from text in _noticeTextRepository.GetAll()
                             where text.ExpireTime > DateTime.Now
                             && ((text.NoticeType == 2 && ((text.NoticeAllUserIds == null || text.NoticeAllUserIds.Length == 0) || text.NoticeAllUserIds.Contains(userId.ToString()))) || text.NoticeType == 3)
                             && !logQuery.Contains(text.Id)
                             select new NoticeNoRead { Id = text.Id, NoticeType = text.NoticeType });

            return textQuery.ToList();
        }


        /// <summary>
        /// 通知公告查看api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<NoticeView> GetForView(NoticeViewInput input)
        {
            var query = await _noticeTextRepository.GetAll().Where(p => p.Id == input.TextId).FirstOrDefaultAsync();
            if (query != null)
            {
                var log = await _noticeLogRepository.GetAll().Where(p => p.Id == input.LogId).FirstOrDefaultAsync();
                if (log != null)
                {
                    log.Status = 2;
                    log.ReadTime = DateTime.Now;
                    await _noticeLogRepository.UpdateAsync(log);
                }
                return new NoticeView
                {
                    TextId = query.Id,
                    Content = query.MsgConent,
                    CreationTime = query.CreationTime,
                    Title = query.Title
                };
            }
            else
            {
                return new NoticeView();
            }
        }
        public async Task<NoticeList> GetNewNotice()
        {
            var queryBase = _noticeTextRepository.GetAll().Where(a => a.NoticeType == 2);
            var newNotice = queryBase.OrderByDescending(a => a.CreationTime).FirstOrDefault();
            if (newNotice == null)
                return null;
            var isFollow = _followRepository.GetAll().FirstOrDefault(x => x.BusinessType == FollowType.公告 && x.CreatorUserId == AbpSession.UserId.Value && x.BusinessId == newNotice.Id) != null;
            var user = _userRepository.GetAll().FirstOrDefault(x=>x.Id==newNotice.CreatorUserId);
            return new NoticeList()
            {
                TextId = newNotice.Id,
                Title = newNotice.Title,
                CreateUserName = user!=null?user.Name:"",
                CreateUserId = newNotice.CreatorUserId.Value,
                Content = newNotice.MsgConent,
                IsFollow = isFollow,
                CreationTime = newNotice.CreationTime
            };
        }
        public async Task<NoticeList> Get(EntityDto<Guid> intput)
        {
            var newNotice = await _noticeTextRepository.GetAll().FirstOrDefaultAsync(r => r.Id == intput.Id);
            if (newNotice == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
            var isFollow = _followRepository.GetAll().FirstOrDefault(x => x.BusinessType == FollowType.公告 && x.CreatorUserId == AbpSession.UserId.Value && x.BusinessId == newNotice.Id) != null;
            var user = _userRepository.GetAll().FirstOrDefault(x=>x.Id==newNotice.CreatorUserId);
            return new NoticeList()
            {
                TextId = newNotice.Id,
                Title = newNotice.Title,
                CreateUserName = user!=null?user.Name:"",
                CreateUserId = newNotice.CreatorUserId.Value,
                Content = newNotice.MsgConent,
                IsFollow = isFollow,
                CreationTime = newNotice.CreationTime
            };
        }
        public async Task<PagedResultDto<NoticeList>> GetNoticeTextsAsync(GetNoticeListInput input)
        {
            var queryBase = _noticeTextRepository.GetAll().Where(a => a.NoticeType == 2);
            if (input.IsNew) { 
            var newNotice = queryBase.OrderByDescending(a => a.CreationTime).FirstOrDefault();
            if (newNotice != null)
                queryBase = queryBase.Where(x => x.Id != newNotice.Id);
            }
            var query =
               (from a in queryBase
                join b in _followRepository.GetAll().Where(x => x.BusinessType == FollowType.公告 && x.CreatorUserId == AbpSession.UserId.Value) on a.Id equals b.BusinessId into tmp1
                from c in tmp1.DefaultIfEmpty()
                join d in _userRepository.GetAll() on a.CreatorUserId equals d.Id
                where (input.IsFollow?c!=null:true)
                select new NoticeList()
                {
                    TextId = a.Id,
                    Title = a.Title,
                    CreateUserName = d.Name,
                    CreateUserId = a.CreatorUserId.Value,
                    Content = a.MsgConent,
                    IsFollow = c != null,
                    CreationTime = a.CreationTime
                }
                );
            if (!string.IsNullOrWhiteSpace(input.SearchKey))
            {
                query = query.Where(r => r.Title.Contains(input.SearchKey) || r.CreateUserName.Contains(input.SearchKey));
            }
            var count = await query.CountAsync();
            var notices = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<NoticeList>() { TotalCount = count, Items = notices };
        }

        /// <summary>
        /// 公司新闻查看api
        /// </summary>
        /// <param name="id">id 为列表上的TextId</param>
        /// <returns></returns>
        public async Task<NoticeView> GetNoticeForEditAsync(Guid? id)
        {
            if (id.HasValue)
            {
                var query = await _noticeTextRepository.GetAll().Where(p => p.Id == id.Value).FirstOrDefaultAsync();
                if (query != null)
                {
                    return new NoticeView
                    {
                        TextId = query.Id,
                        Content = query.MsgConent,                     
                        CreationTime = query.CreationTime,
                        Title = query.Title,
                        NoticeDepartmentIds = query.NoticeDepartmentIds,
                        NoticeGroupIds = query.NoticeGroupIds,
                        NoticeUserIds = query.NoticeUserIds
                    };
                }
                else
                {
                    return new NoticeView();
                }
            }
            else
            {
                return new NoticeView() { };

            }

        }




        public async Task DeleteNoticeAsync(EntityDto<Guid> intput)
        {
            var noticemodel = await _noticeTextRepository.GetAll().FirstOrDefaultAsync(r => r.Id == intput.Id);
            if (noticemodel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
            else
            {
                var logmodels = await _noticeLogRepository.GetAll().Where(r => r.TextId == intput.Id).ToListAsync();
                logmodels.ForEach(r => { _noticeLogRepository.DeleteAsync(r); });
                await _noticeTextRepository.DeleteAsync(noticemodel);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }


    }
}