using Abp.Application.Services;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.SignalR.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ZCYX.FRMSCore.Model;

namespace ZCYX.FRMSCore.Application
{
    [RemoteService(IsEnabled = false)]
    public class ProjectNoticeManager : ApplicationService
    {
        private readonly IRepository<NoticeTexts, Guid> _noticeTextRepository;
        private readonly IRepository<NoticeLogs, Guid> _noticeLogRepository;
        private readonly ISignalrNoticeAppService _noticeCommunicator;
        private readonly IOnlineClientManager _onlineClientManager;

        public ProjectNoticeManager(IRepository<NoticeTexts, Guid> noticeTextRepository, IRepository<NoticeLogs, Guid> noticeLogRepository,
            IOnlineClientManager onlineClientManager,
            ISignalrNoticeAppService noticeCommunicator)
        {
            _noticeTextRepository = noticeTextRepository;
            _noticeLogRepository = noticeLogRepository;
            _noticeCommunicator = noticeCommunicator;
            _onlineClientManager = onlineClientManager;
        }

        public async Task SendPrivateMessageAsync(long? receiveId, Guid? projectId, string title, string content, string link = "")
        {
            var text_id = Guid.NewGuid();
            var text = new NoticeTexts
            {
                SendUserId = 0,
                ProjectId = projectId,
                Title = title,
                MsgConent = content,
                NoticeType = (int)SystemNoticeType.事务通知,
                CreationTime = DateTime.Now,
                ExpireTime = new DateTime(2099, 09, 09),
                LinkUrl = link,
                Id = text_id
            };

            await _noticeTextRepository.InsertAsync(text);
            var log = new NoticeLogs
            {
                TextId = text_id,
                ReceiveId = receiveId.Value,
                Status = 1,
                CreationTime = DateTime.Now,
                ReadTime = new DateTime(1900, 01, 01),
                NoticeType = (int)SystemNoticeType.事务通知,
            };
            await _noticeLogRepository.InsertAsync(log);

        }



        /// <summary>
        /// 发布通知公告 和 发布公司新闻
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task CreateOrUpdateNoticeAsync(NoticePublishInput input)
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
        
        /// <summary>
        /// 发布通知公告 和 发布公司新闻
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public  void CreateOrUpdateNotice(NoticePublishInput input)
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
                 UpdateNoticeSync(input);
            }
            else
            {
                CreateNoticeSync(input, userids);
            }
        }



        protected  void CreateNoticeSync(NoticePublishInput input, List<long> users = null)
        {
            if (input.Id.HasValue)
            {

            }
            var userId =input.SendUserId.HasValue?input.SendUserId.Value:AbpSession.GetUserId();

            var textId =  _noticeTextRepository.InsertAndGetId(new NoticeTexts
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
                CreatorUserId=userId,
                NoticeAllUserIds = users == null ? null : string.Join(",", users)
            });



            ///无论什么类型  只要user.count=0 则为全体员工都接收；  zcl 2018.8.31
            if (users.Count == 0)
            {
                _noticeLogRepository.Insert(new NoticeLogs
                {
                    TextId = textId,
                    ReceiveId = userId,
                    Status = 1,
                    CreatorUserId = userId,
                    NoticeType = input.NoticeType,
                    CreationTime = DateTime.Now
                });
            }
            else
            {
                foreach (var item in users)
                {
                    _noticeLogRepository.Insert(new NoticeLogs
                    {
                        TextId = textId,
                        ReceiveId = item,
                        Status = 1,
                        CreatorUserId = userId,
                        NoticeType = input.NoticeType,
                        CreationTime = DateTime.Now
                    });
                }
            }




            //日志功能暂时屏蔽
            //var logservice = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Logapp.ILogAppService>();
            //await logservice.CreateOrUpdateLogV2("发布公告", string.Format("发布公告，标题：{0}", input.Title), SystemLogTypes.日常办公.ToString());
             CurrentUnitOfWork.SaveChanges();


            var onlineClients = new List<IOnlineClient>();
            if (users.Count > 0)
            {
                onlineClients = _onlineClientManager.GetAllClients().Where(p =>p.UserId.HasValue&& p.UserId.Value != userId && users.Contains(p.UserId.Value)).ToList();
            }
            else
            {
                onlineClients = _onlineClientManager.GetAllClients().Where(p => p.UserId.HasValue && p.UserId.Value != userId).ToList();
            }

            _noticeCommunicator.SendNoticeToClient(onlineClients, null, input.Title, string.Empty);
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



            ///无论什么类型  只要user.count=0 则为全体员工都接收；  zcl 2018.8.31
            if (users.Count == 0)
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




            //日志功能暂时屏蔽
            //var logservice = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Logapp.ILogAppService>();
            //await logservice.CreateOrUpdateLogV2("发布公告", string.Format("发布公告，标题：{0}", input.Title), SystemLogTypes.日常办公.ToString());
            await CurrentUnitOfWork.SaveChangesAsync();


            var onlineClients = new List<IOnlineClient>();
            if (users.Count > 0)
            {
                onlineClients = _onlineClientManager.GetAllClients().Where(p =>p.UserId.HasValue&& p.UserId.Value != userId && users.Contains(p.UserId.Value)).ToList();
            }
            else
            {
                onlineClients = _onlineClientManager.GetAllClients().Where(p => p.UserId.HasValue && p.UserId.Value != userId).ToList();
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
        protected  void UpdateNoticeSync(NoticePublishInput input)
        {
            var noticemodel =  _noticeTextRepository.GetAll().FirstOrDefault(r => r.Id == input.Id);
            if (noticemodel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
            else
            {
                noticemodel.Title = input.Title;
                noticemodel.MsgConent = input.Content;
                noticemodel.NoticeDepartmentIds = input.NoticeDepartmentIds;
                noticemodel.NoticeGroupIds = input.NoticeGroupIds;
                noticemodel.NoticeUserIds = input.NoticeUserIds;
                 _noticeTextRepository.Update(noticemodel);
                //var logservice = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<Logapp.ILogAppService>();
                //await logservice.CreateOrUpdateLogV2("更新公告", string.Format("更新公告，标题：{0}", input.Title), SystemLogTypes.日常办公.ToString());
            }

        }
    }
}
