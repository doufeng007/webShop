using System;
using System.Collections.Generic;
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
using ZCYX.FRMSCore;
using Abp.Extensions;
using Abp.UI;
using ZCYX.FRMSCore.Application;
using Abp.WorkFlow;
using Abp.Application.Services;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace HR
{
    public class OrganizationUnitPostPlanAppService : FRMSCoreAppServiceBase, IOrganizationUnitPostPlanAppService
    {
        private readonly IRepository<OrganizationUnitPostPlan, Guid> _repository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostRepository;
        private readonly IRepository<OrganizationUnitPostChangePlan, Guid> _organizationUnitPostChangePlanRepository;

        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<PostInfo, Guid> _postInforepository;
        private readonly IRepository<UserPosts, Guid> _userPostsrepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;

        public OrganizationUnitPostPlanAppService(IRepository<OrganizationUnitPostPlan, Guid> repository
            , IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<PostInfo, Guid> postInforepository
            , IRepository<UserPosts, Guid> userPostsrepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IRepository<OrganizationUnitPosts, Guid> organizationUnitPostRepository
            , IRepository<OrganizationUnitPostChangePlan, Guid> organizationUnitPostChangePlanRepository
            , ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager
            , IWorkFlowTaskRepository workFlowTaskRepository)
        {
            this._repository = repository;
            _organizationUnitRepository = organizationUnitRepository;
            _postInforepository = postInforepository;
            _userPostsrepository = userPostsrepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _organizationUnitPostRepository = organizationUnitPostRepository;
            _organizationUnitPostChangePlanRepository = organizationUnitPostChangePlanRepository;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrganizationUnitPostPlanListOutputDto>> GetList(GetOrganizationUnitPostPlanListInput input)
        {
            var ret = new List<OrganizationUnitPostPlanListOutputDto>();
            var toalCount = 0;
            if (input.ActionType == 1)
            {
                var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                            join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                            join c in _postInforepository.GetAll() on a.PostId equals c.Id
                            let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                              x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                              x.ReceiveID == AbpSession.UserId.Value)
                                             select c)
                            select new OrganizationUnitPostPlanListOutputDto()
                            {
                                Id = a.Id,
                                OrganizationUnitId = a.OrganizationUnitId,
                                OrganizationUnitName = b.DisplayName,
                                PostId = a.PostId,
                                PostName = c.Name,
                                PrepareNumber = a.PrepareNumber,
                                CreationTime = a.CreationTime,
                                Status = a.Status,
                                OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                            };
                query = query.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), r => r.OrganizationUnitName.Contains(input.SearchKey) || r.PostName.Contains(input.SearchKey));
                toalCount = await query.CountAsync();
                ret = await query.OrderBy(r => r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            }
            else
            {
                var query = from a in _organizationUnitPostChangePlanRepository.GetAll().Where(x => !x.IsDeleted)
                            join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                            join c in _postInforepository.GetAll() on a.PostId equals c.Id
                            let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                          x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                          x.ReceiveID == AbpSession.UserId.Value)
                                             select c)
                            select new OrganizationUnitPostPlanListOutputDto()
                            {
                                Id = a.Id,
                                OrganizationUnitId = a.OrganizationUnitId,
                                OrganizationUnitName = b.DisplayName,
                                PostId = a.PostId,
                                PostName = c.Name,
                                PrepareNumber = a.PrepareNumber,
                                CreationTime = a.CreationTime,
                                Status = a.Status,
                                OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                            };
                query = query.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), r => r.OrganizationUnitName.Contains(input.SearchKey) || r.PostName.Contains(input.SearchKey));
                toalCount = await query.CountAsync();
                ret = await query.OrderBy(r=>r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            }

            foreach (var item in ret)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);
            }
            return new PagedResultDto<OrganizationUnitPostPlanListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OrganizationUnitPostPlanOutputDto> Get(GetOrgPostPlanInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            if (input.ActionType == 1)
            {
                var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                            join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                            join c in _postInforepository.GetAll() on a.PostId equals c.Id
                            where a.Id == id
                            select new OrganizationUnitPostPlanOutputDto()
                            {
                                Id = a.Id,
                                OrganizationUnitId = a.OrganizationUnitId,
                                OrganizationUnitName = b.DisplayName,
                                PostId = a.PostId,
                                PostName = c.Name,
                                PrepareNumber = a.PrepareNumber,
                                CreationTime = a.CreationTime
                            };
                var model = await query.FirstOrDefaultAsync();
                if (model == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                return model;
            }
            else
            {
                var query = from a in _organizationUnitPostChangePlanRepository.GetAll().Where(x => !x.IsDeleted)
                            join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                            join c in _postInforepository.GetAll() on a.PostId equals c.Id
                            where a.Id == id
                            select new OrganizationUnitPostPlanOutputDto()
                            {
                                Id = a.Id,
                                OrganizationUnitId = a.OrganizationUnitId,
                                OrganizationUnitName = b.DisplayName,
                                PostId = a.PostId,
                                PostName = c.Name,
                                PrepareNumber = a.PrepareNumber,
                                CreationTime = a.CreationTime
                            };
                var model = await query.FirstOrDefaultAsync();
                if (model == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                return model;
            }
        }
        /// <summary>
        /// 添加一个OrganizationUnitPosts
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateOrganizationUnitPostPlanInput input)
        {
            var newId = Guid.NewGuid();
            if (input.ActionType == 1)
            {
                if (input.PostId.HasValue)
                {
                    var exit_model = _organizationUnitPostRepository.GetAll().Any(r => r.OrganizationUnitId == input.OrganizationUnitId && r.PostId == input.PostId.Value);
                    if (exit_model)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"该部门下已存在:{input.PostName} 编制!");
                    var exit_Model_Plan = _repository.GetAll().Any(r => r.OrganizationUnitId == input.OrganizationUnitId && r.PostId == input.PostId.Value);
                    if (exit_Model_Plan)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"该部门下{input.PostName} 编制正在审批中，请等待领导审批结果!");
                    var entity = new OrganizationUnitPostPlan() { Id = newId, OrganizationUnitId = input.OrganizationUnitId, PostId = input.PostId.Value, PrepareNumber = input.PrepareNumber };
                    await _repository.InsertAsync(entity);
                }
                else
                {
                    var postService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IPostInfoAppService>();
                    var exit_ModelByName = await postService.GetByNameAsync(input.PostName);
                    if (exit_ModelByName != null)
                    {
                        var exit_model = _organizationUnitPostRepository.GetAll().Any(r => r.OrganizationUnitId == input.OrganizationUnitId && r.PostId == input.PostId.Value);
                        if (exit_model)
                            throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"该部门下已存在:{input.PostName} 编制!");
                        var exit_Model_Plan = _repository.GetAll().Any(r => r.OrganizationUnitId == input.OrganizationUnitId && r.PostId == input.PostId.Value);
                        if (exit_Model_Plan)
                            throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"该部门下{input.PostName} 编制正在审批中，请等待领导审批结果!");
                        var entity = new OrganizationUnitPostPlan() { Id = newId, OrganizationUnitId = input.OrganizationUnitId, PostId = exit_ModelByName.Id, PrepareNumber = input.PrepareNumber };
                        await _repository.InsertAsync(entity);
                    }
                    else
                    {
                        var new_post = new PostInfo() { Id = Guid.NewGuid(), Name = input.PostName };
                        await _postInforepository.InsertAsync(new_post);
                        var entity = new OrganizationUnitPostPlan() { Id = newId, OrganizationUnitId = input.OrganizationUnitId, PostId = new_post.Id, PrepareNumber = input.PrepareNumber };
                        await _repository.InsertAsync(entity);
                    }
                }
            }
            else if (input.ActionType == 2)
            {
                if (input.PostId.HasValue)
                {
                    var exit_Model_Plan = _organizationUnitPostChangePlanRepository.GetAll().Any(r => r.OrganizationUnitId == input.OrganizationUnitId && r.PostId == input.PostId.Value && r.Status != -1);
                    if (exit_Model_Plan)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"该部门下{input.PostName} 编制正在审批中，请等待领导审批结果!");
                    var entity = new OrganizationUnitPostChangePlan() { Id = newId, OrganizationUnitId = input.OrganizationUnitId, PostId = input.PostId.Value, PrepareNumber = input.PrepareNumber };
                    await _organizationUnitPostChangePlanRepository.InsertAsync(entity);
                }
                else
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "参数异常");
                }
            }


            return new InitWorkFlowOutput() { InStanceId = newId.ToString() };
        }

        /// <summary>
        /// 修改一个OrganizationUnitPosts
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateOrganizationUnitPostPlanInput input)
        {
            if (input.Id != Guid.Empty)
            {

                if (input.ActionType == 1)
                {


                    var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                    if (dbmodel == null)
                    {
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                    }
                    var old_Model = dbmodel.DeepClone();
                    dbmodel.OrganizationUnitId = input.OrganizationUnitId;

                    dbmodel.PrepareNumber = input.PrepareNumber;

                    await _repository.UpdateAsync(dbmodel);

                    if (input.IsUpdateForChange)
                    {
                        var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                        if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                        var logs = new OrganizationUnitPostPlanChangeDto() { PrepareNumber = old_Model.PrepareNumber }.GetColumnAllLogs(new OrganizationUnitPostPlanChangeDto()
                        { PrepareNumber = dbmodel.PrepareNumber });
                        await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
                    }

                }
                else
                {
                    var dbmodel = await _organizationUnitPostChangePlanRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                    if (dbmodel == null)
                    {
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                    }
                    var old_Model = dbmodel.DeepClone();
                    dbmodel.OrganizationUnitId = input.OrganizationUnitId;
                    dbmodel.PrepareNumber = input.PrepareNumber;
                    await _organizationUnitPostChangePlanRepository.UpdateAsync(dbmodel);

                    if (input.IsUpdateForChange)
                    {
                        var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                        if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                        var logs = new OrganizationUnitPostPlanChangeDto() { PrepareNumber = old_Model.PrepareNumber }.GetColumnAllLogs(new OrganizationUnitPostPlanChangeDto()
                        { PrepareNumber = dbmodel.PrepareNumber });
                        await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
                    }
                }




            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }


        /// <summary>
        /// 新增编制的完成事件
        /// </summary>
        /// <param name="instanceId"></param>
        [RemoteService(IsEnabled = false)]
        public void CompletaPlan(string instanceId)
        {
            var id = instanceId.ToGuid();
            var model = _repository.Get(id);
            var exit_model = _organizationUnitPostRepository.GetAll().SingleOrDefault(r => r.OrganizationUnitId == model.OrganizationUnitId && r.PostId == model.PostId);
            if (exit_model == null)
            {
                var entity = new OrganizationUnitPosts()
                {
                    Id = Guid.NewGuid(),
                    PostId = model.PostId,
                    OrganizationUnitId = model.OrganizationUnitId,
                    PrepareNumber = model.PrepareNumber,
                };
                _organizationUnitPostRepository.Insert(entity);
            }
            else
            {
                exit_model.PrepareNumber = model.PrepareNumber;
            }

        }


        /// <summary>
        /// 整改编制的完成事件
        /// </summary>
        /// <param name="instanceId"></param>
        [RemoteService(IsEnabled = false)]
        public void CompletaChangePlan(string instanceId)
        {
            var id = instanceId.ToGuid();
            var model = _organizationUnitPostChangePlanRepository.Get(id);
            var exit_model = _organizationUnitPostRepository.GetAll().SingleOrDefault(r => r.OrganizationUnitId == model.OrganizationUnitId && r.PostId == model.PostId);
            if (exit_model == null)
            {
                var entity = new OrganizationUnitPosts()
                {
                    Id = Guid.NewGuid(),
                    PostId = model.PostId,
                    OrganizationUnitId = model.OrganizationUnitId,
                    PrepareNumber = model.PrepareNumber,
                };
                _organizationUnitPostRepository.Insert(entity);
            }
            else
            {
                exit_model.PrepareNumber = model.PrepareNumber;
            }

        }



        // <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }
    }
}