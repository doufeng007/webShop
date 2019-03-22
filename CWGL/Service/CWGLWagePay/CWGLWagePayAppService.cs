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
using Microsoft.Extensions.Configuration;
using Abp.Reflection.Extensions;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.Model;

namespace CWGL
{
    public class CWGLWagePayAppService : FRMSCoreAppServiceBase, ICWGLWagePayAppService
    {
        private readonly IRepository<CWGLWagePay, Guid> _repository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        public CWGLWagePayAppService(IRepository<CWGLWagePay, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager
            , WorkFlowCacheManager workFlowCacheManager, IWorkFlowWorkTaskAppService workFlowWorkTaskAppService, IWorkFlowTaskRepository workFlowTaskRepository
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            var coreAssemblyDirectoryPath = typeof(CWGLWagePayAppService).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CWGLWagePayListOutputDto>> GetList(GetCWGLWagePayListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new CWGLWagePayListOutputDto()
                        {
                            Id = a.Id,
                            WageDate = a.WageDate,
                            DoTime = a.DoTime,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r=>r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret) { item.InstanceId = item.Id.ToString(); _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item); }
            return new PagedResultDto<CWGLWagePayListOutputDto>(toalCount, ret);
        }


        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<CWGLWagePayOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var ret = model.MapTo<CWGLWagePayOutputDto>();
            return ret;
        }


        /// <summary>
        /// 添加一个CWGLWagePay
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCWGLWagePayInput input)
        {
            var newmodel = new CWGLWagePay()
            {
                WageDate = input.WageDate,
            };
            newmodel.Status = 0;
            await _repository.InsertAsync(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        public void AutoCreate()
        {
            var wageFlowId = _appConfiguration["App:WageFlowId"].ToGuid();
            var wageRoleId = _appConfiguration["App:WageRoleId"].ToInt();
            var date = DateTime.Now.AddMonths(-1);
            var newModel = new CWGLWagePay()
            {
                Id = Guid.NewGuid(),
                WageDate = date,
            };
            var flowTitle = $"{date.Year}年{date.Month}月工资发放";
            _repository.Insert(newModel);
            _workFlowWorkTaskAppService.InitWorkFlowInstanceByRole(new InitWorkFlowInput() { FlowId = wageFlowId, FlowTitle = flowTitle, InStanceId = newModel.Id.ToString() }, 1, wageRoleId);
        }

        /// <summary>
        /// 修改一个CWGLWagePay
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateCWGLWagePayInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var logModel = new CWGLWagePay();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<CWGLWagePay>();
                }
                dbmodel.WageDate = input.WageDate;
                input.FACData.BusinessId = input.Id.ToString();
                await _repository.UpdateAsync(dbmodel);
                var groupId = Guid.NewGuid();
                input.FACData.GroupId = groupId;
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));                 
                    await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table, groupId);
                }
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }
        private CWGLWagePayLogDto GetChangeModel(CWGLWagePay model)
        {
            var ret = model.MapTo<CWGLWagePayLogDto>();
            return ret;
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