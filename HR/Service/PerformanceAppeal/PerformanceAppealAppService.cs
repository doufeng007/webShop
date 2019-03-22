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
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Authorization.Users;
using Abp;

namespace HR
{
    public class PerformanceAppealAppService : FRMSCoreAppServiceBase, IPerformanceAppealAppService
    {
        private readonly IRepository<PerformanceAppeal, Guid> _repository;
        private readonly IRepository<PerformanceAppealDetail, Guid> _detailRepository;
        private readonly IRepository<Performance, Guid> _performanceRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<PostInfo, Guid> _postInfoRepository;
        public PerformanceAppealAppService(IRepository<PerformanceAppeal, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<WorkFlowTask, Guid> workFlowTaskRepository, IRepository<PerformanceAppealDetail, Guid> detailRepository, IRepository<Performance, Guid> performanceRepository, IRepository<User, long> userRepository, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<PostInfo, Guid> postInfoRepository
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _detailRepository = detailRepository;
            _userRepository = userRepository;
            _performanceRepository = performanceRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _postInfoRepository = postInfoRepository;
        }



        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<PerformanceAppealListOutputDto>> GetMeList(GetPerformanceAppealListInput input)
        {
            var queryBase = _detailRepository.GetAll();
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.CreatorUserId==AbpSession.UserId.Value)
                        let c1 = queryBase.Count(x => x.AppealId == a.Id && x.Type == PerformanceType.非数字化绩效考核)
                        let c2 = queryBase.Count(x => x.AppealId == a.Id && x.Type == PerformanceType.数字化绩效考核)
                        let openModel = (from b in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() && x.ReceiveID == AbpSession.UserId.Value) select b)
                        select new PerformanceAppealListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            NoDataCount = c1,
                            DataCount = c2,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0 ? 1 : 2,
                        };
            var toalCount = query.Count();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret) { item.InstanceId = item.Id.ToString(); _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item); }
            return new PagedResultDto<PerformanceAppealListOutputDto>(toalCount, ret);
        }
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<PerformanceAppealListOutputDto>> GetList(GetPerformanceAppealListInput input)
        {
            var queryBase = _detailRepository.GetAll();
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join u in UserManager.Users on a.CreatorUserId equals u.Id
                        join m in _organizationUnitRepository.GetAll() on a.OrgId equals m.Id
                        let cc = (from o in _postInfoRepository.GetAll()
                                  where a.PostIds.GetStrContainsArray(o.Id.ToString())
                                  select new WorkTempPost { PostId = o.Id, PostName = o.Name }).ToList()
                        let c1= queryBase.Count(x=>x.AppealId==a.Id && x.Type== PerformanceType.非数字化绩效考核)
                        let c2= queryBase.Count(x=>x.AppealId==a.Id && x.Type== PerformanceType.数字化绩效考核)
                        let openModel = (from b in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() && x.ReceiveID == AbpSession.UserId.Value) select b)
                        where a.Status > 0
                        select new PerformanceAppealListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            DepartmentName = m.DisplayName,
                            Post = cc,
                            NoDataCount =c1,
                            DataCount=c2,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0 ? 1 : 2,
                        };
            var toalCount = query.Count();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret) { item.InstanceId = item.Id.ToString(); _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item); }
            return new PagedResultDto<PerformanceAppealListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		[Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
		public PerformanceAppealOutputDto Get(GetWorkFlowTaskCommentInput input)
		{
			var id = Guid.Parse(input.InstanceId);
		    var model = _repository.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<PerformanceAppealOutputDto>();
		}
        /// <summary>
        /// 添加一个PerformanceAppeal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreatePerformanceAppealInput input)
        {
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var userOrgModel = await service.GetUserPostInfo(new NullableIdDto<long>() { Id = AbpSession.UserId.Value, }, new NullableIdDto<long>() { Id = null });
            var id = Guid.NewGuid();
            var newmodel = new PerformanceAppeal();
            newmodel.Status = 0;
            newmodel.Id = id;
            newmodel.OrgId = userOrgModel.OrgId;
            newmodel.PostIds = string.Join(",", userOrgModel.UserPosts.Select(r => r.PostId));
            _repository.Insert(newmodel);
            foreach (var item in input.performanceAppealDetails)
            {
                var detailModel = new PerformanceAppealDetail();
                var model = _performanceRepository.GetAll().FirstOrDefault(x => x.Id == item.PerformanceId);
                if (model == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该绩效不存在！");
                }
                detailModel.PerformanceId = item.PerformanceId;
                detailModel.Type = item.Type;
                detailModel.Content = item.Content;
                detailModel.AppealId = id;
                detailModel.Score = model.Score;
                _detailRepository.Insert(detailModel);
            }
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个PerformanceAppeal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdatePerformanceAppealInput input)
        {
            foreach (var item in input.performanceAppealDetails)
            {
                var model = _performanceRepository.GetAll().FirstOrDefault(x => x.Id == item.PerformanceId);
                if (model != null)
                {
                    model.Score = item.Score;
                    await _performanceRepository.UpdateAsync(model);
                }
                var detailModel = _detailRepository.GetAll().FirstOrDefault(x => x.Id == item.Id);
                if (detailModel != null)
                {
                    detailModel.AfterScore = item.Score;
                    await _detailRepository.UpdateAsync(detailModel);
                }

            }
        }
		private PerformanceAppealLogDto GetChangeModel(PerformanceAppeal model)
        {
            var ret = model.MapTo<PerformanceAppealLogDto>();
            return ret;
        }
		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public void Delete(EntityDto<Guid> input)
        {
            _repository.Delete(x=>x.Id == input.Id);
        }
    }
}