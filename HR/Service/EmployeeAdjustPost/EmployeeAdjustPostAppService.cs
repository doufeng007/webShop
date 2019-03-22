using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.WorkFlow;
using HR.Service.EmployeeAdjustPost.Dto;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Authorization.Users;

namespace HR
{
    [AbpAuthorize]
    public class EmployeeAdjustPostAppService : FRMSCoreAppServiceBase, IEmployeeAdjustPostAppService
    { 
        private readonly IRepository<EmployeeAdjustPost, Guid> _repository;
        private readonly IRepository<UserPosts, Guid> _userPostRepository;
        private readonly IRepository<User, long> _usersRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitPostsRepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IRepository<PostInfo, Guid> _postRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        public EmployeeAdjustPostAppService(IRepository<EmployeeAdjustPost, Guid> repository,
            IRepository<UserPosts, Guid> userPostRepository, IWorkFlowTaskRepository workFlowTaskRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager,
            IRepository<PostInfo, Guid> postRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager,
            IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitPostsRepository
            , ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<User, long> usersRepository)
        {
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            this._repository = repository;
            _organizeRepository = organizeRepository;
            _userPostRepository = userPostRepository;
            _postRepository = postRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _organizationUnitPostsRepository = organizationUnitPostsRepository;
            _userOrganizationUnitPostsRepository = userOrganizationUnitPostsRepository;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<EmployeeAdjustPostListOutputDto>> GetList(GetEmployeeAdjustPostListInput input)
        {
            //var user = await base.GetCurrentUserAsync();

            var query = from a in _repository.GetAll()
                         join b in _organizeRepository.GetAll() on a.OriginalDepId equals b.Id
                         join c in _organizeRepository.GetAll() on a.AdjustDepId equals c.Id
                         join d in _postRepository.GetAll() on a.OriginalPostId equals d.Id
                         join e in _organizationUnitPostsRepository.GetAll() on a.AdjustPostId equals e.Id
                         join f in _postRepository.GetAll() on e.PostId equals f.Id
                         join g in _usersRepository.GetAll() on a.CreatorUserId equals g.Id
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                       
                         select new EmployeeAdjustPostListOutputDto()
                         {
                             AdjustDepId = a.AdjustDepId,
                              CreatorUserId=a.CreatorUserId,
                             AdjustDepName = c.DisplayName,
                             AdjustPostId = a.AdjustPostId,
                             AdjustPostName = f.Name,
                             CreationTime = a.CreationTime,
                             DepartmentName = b.DisplayName,
                             Id = a.Id,
                              Status=a.Status.HasValue?a.Status.Value:0,
                             PostName = d.Name,
                             Remark = a.Remark,
                             UserName = g.Name,
                             WorkNumber = g.WorkNumber,
                             OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                         };
            if (input.ShowMyCase==0) {
                query = query.Where(ite => ite.CreatorUserId== AbpSession.UserId.Value);
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret){
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);
            }
            return new PagedResultDto<EmployeeAdjustPostListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<EmployeeAdjustPostOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await (from a in _repository.GetAll().Where(x => x.Id == id)
                               
                join b in base.UserManager.Users on a.CreatorUserId equals b.Id
                join d1 in _postRepository.GetAll().Where(x => !x.IsDeleted).DefaultIfEmpty() on a.OriginalPostId equals
                    d1.Id into tmp1 //原有岗位
                from d in tmp1.DefaultIfEmpty()
                join e1 in _organizeRepository.GetAll().Where(x => !x.IsDeleted).DefaultIfEmpty() on a.OriginalDepId
                    equals e1.Id into tmp2 //原有部门
                from e in tmp2.DefaultIfEmpty()
                join f1 in _organizationUnitPostsRepository.GetAll().Where(x => !x.IsDeleted).DefaultIfEmpty() on a.AdjustPostId equals
                    f1.Id into tmp3 //调入岗位
               
                join g1 in _organizeRepository.GetAll().Where(x => !x.IsDeleted).DefaultIfEmpty() on a.AdjustDepId
                    equals g1.Id into tmp4 //调入部门
                from g in tmp4.DefaultIfEmpty()
                               from f in tmp3.DefaultIfEmpty()
                               select new EmployeeAdjustPostOutputDto()
                {
                    Id = a.Id,
                    Remark = a.Remark,
                    DepartmentName = e == null ? "" : e.DisplayName,
                    PostName = d == null ? "" : d.Name,
                    AdjustDepId = a.AdjustDepId,
                    AdjustPostId = a.AdjustPostId,
                     PostId=f.PostId,
                    WorkNumber = b.WorkNumber,
                    AdjustDepName = g == null ? "" : g.DisplayName,
                                   UserName = b.Name,
                    CreationTime = a.CreationTime
                }).FirstOrDefaultAsync();

            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            model.AdjustPostName = _postRepository.Get(model.PostId).Name;
            return model;
        }

        /// <summary>
        /// 添加一个EmployeeAdjustPost
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateEmployeeAdjustPostInput input)
        {
            var user = await base.GetCurrentUserAsync();
            var userPost = _userPostRepository.FirstOrDefault(x=>x.UserId==user.Id);
            if (userPost == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "当前登录账户没有部门和岗位信息,无法发起调岗申请。");
            }
            if ((await _repository.CountAsync(x => !x.IsDeleted && x.CreatorUserId== user.Id && x.Status != -1 && x.Status != -2)) > 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "你尚有其他调岗申请未结束,无法发起新的调岗申请。");
            }
            if (await _organizationUnitPostsRepository.CountAsync(x => x.Id == input.AdjustPostId) == 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "没有找到您选择的岗位。");
            }
            var nowpost = _userPostRepository.GetAll().FirstOrDefault(ite => ite.UserId == user.Id && ite.OrgPostId == input.AdjustPostId);
            if (nowpost != null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "你已经在该岗位了，不能再次调入哦。");
            }
            var newmodel = new EmployeeAdjustPost()
            {
                Remark = input.Remark,
                OriginalDepId = userPost.OrgId,
                OriginalPostId = userPost.PostId,
                WorkflowAdjsutDepId = input.AdjustDepId,
                AdjustDepId = Convert.ToInt32(MemberPerfix.RemovePrefix(input.AdjustDepId)?.Split(',')[0]),
                AdjustPostId = input.AdjustPostId
            };
            if (await _organizeRepository.CountAsync(x => x.Id == newmodel.AdjustDepId) == 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "没有找到您选择的部门。");
            }
            //input.AdjustDepId
            //input.AdjustPostId
            await _repository.InsertAsync(newmodel);
            return new InitWorkFlowOutput() {InStanceId = newmodel.Id.ToString()};
        }

        /// <summary>
        /// 修改一个EmployeeAdjustPost
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateEmployeeAdjustPostInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var logModel = new EmployeeAdjustPost();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<EmployeeAdjustPost>();
                }
                var AdjustDepId = Convert.ToInt32(MemberPerfix.RemovePrefix(input.AdjustDepId)?.Split(',')[0]);
                if (await _organizeRepository.CountAsync(x => x.Id == AdjustDepId) == 0)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "没有找到您选择的岗位。");
                }
                dbmodel.Remark = input.Remark;
                dbmodel.WorkflowAdjsutDepId = input.AdjustDepId;
                dbmodel.AdjustDepId = Convert.ToInt32(MemberPerfix.RemovePrefix(input.AdjustDepId)?.Split(',')[0]);
                dbmodel.AdjustPostId = input.AdjustPostId;
                await _repository.UpdateAsync(dbmodel);
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }
        private EmployeeAdjustPostLogDto GetChangeModel(EmployeeAdjustPost model)
        {
            var ret = model.MapTo<EmployeeAdjustPostLogDto>();
            ret.AdjustDepName = _organizeRepository.GetAll()
                .FirstOrDefault(x => !x.IsDeleted && x.Id == model.AdjustDepId)?.DisplayName;
            ret.AdjustPostName = _postRepository.GetAll()
                .FirstOrDefault(x => !x.IsDeleted && x.Id == model.AdjustPostId)?.Name;
            return ret;
        }
        /// <summary>
        /// 调岗事件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        public bool AdjustPostRun(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var dbmodel = _repository.FirstOrDefault(x => x.Id == Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var r= _workFlowOrganizationUnitsManager.SetUserPost(dbmodel.CreatorUserId.Value,new List<Guid>() { dbmodel.AdjustPostId },dbmodel.AdjustPostId).Result;
                return r;
                ////删除旧有数据
                //var userdep = _userOrganizationUnitPostsRepository.GetAll()
                //    .Where(x => x.IsMain && x.UserId == dbmodel.CreatorUserId && !x.IsDeleted).ToList();
                //userdep.ForEach(x =>
                //{
                //    var userPost = _userPostRepository.GetAll().Where(y => y.UserId == dbmodel.CreatorUserId && y.OrgId == x.OrganizationUnitId).ToList();
                //    userPost.ForEach(y =>
                //    {
                //        y.IsDeleted = true;
                //        _userPostRepository.Update(y);
                //    });
                //    x.IsDeleted = true;
                //    _userOrganizationUnitPostsRepository.Update(x);
                //});
                ////调岗到新岗位
                //var orgPostModel =  _organizationUnitPostsRepository.FirstOrDefault(x =>
                //    x.OrganizationUnitId == dbmodel.AdjustDepId && x.PostId == dbmodel.AdjustPostId && !x.IsDeleted);
                //if (orgPostModel != null)
                //{
                //    var newPost = new UserPosts()
                //    {
                //        Id = Guid.NewGuid(),
                //        UserId = dbmodel.CreatorUserId.Value,
                //        PostId = dbmodel.AdjustPostId,
                //        OrgPostId = orgPostModel.Id,
                //        OrgId = dbmodel.AdjustDepId
                //    };
                //    _userPostRepository.Insert(newPost);
                //    var organize = new WorkFlowUserOrganizationUnits()
                //    {
                //        OrganizationUnitId = dbmodel.AdjustDepId,
                //        UserId = dbmodel.CreatorUserId.Value,
                //        IsMain = true
                //    };
                //    _userOrganizationUnitPostsRepository.Insert(organize);
                //    return true;
                //}
                //else
                //{
                //    throw new UserFriendlyException("所选部门岗位不存在。");
                //}
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }


        public bool EmployeeAdjustPostIsLeader(Guid guid)
        {
            var model = _repository.GetAll().FirstOrDefault(x => x.Id == guid);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            var upost = _userPostRepository.GetAll().FirstOrDefault(ite => ite.UserId == model.CreatorUserId&&ite.IsMain);//主岗位
            if (upost == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该用户未设置主岗位，无法调岗！");
            var post = _organizationUnitPostsRepository.Get(upost.OrgPostId);
            if (post.Level == 0)
            {
                return true;//领导
            }
            else {
                return false;//非领导
            }
            //var user = _usersRepository.GetAll().FirstOrDefault(x => x.Id == model.CreatorUserId);
            //if (user == null)
            //    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "用户数据不存在！");
            //var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            //return manager.IsChargerLeaderOrDivision(user.Id);
        }
    }
}