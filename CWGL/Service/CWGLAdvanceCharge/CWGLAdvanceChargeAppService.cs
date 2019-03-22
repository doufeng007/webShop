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
using CWGL.Enums;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace CWGL
{
    public class CWGLAdvanceChargeAppService : FRMSCoreAppServiceBase, ICWGLAdvanceChargeAppService
    {
        private readonly IRepository<CWGLAdvanceCharge, Guid> _repository;
        private readonly IRepository<CWGLAdvanceChargeDetail, Guid> _advanceChargeDetailRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        public CWGLAdvanceChargeAppService(IRepository<CWGLAdvanceCharge, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService, ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<CWGLAdvanceChargeDetail, Guid> advanceChargeDetailRepository, IWorkFlowTaskRepository workFlowTaskRepository)
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _advanceChargeDetailRepository = advanceChargeDetailRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CWGLAdvanceChargeListOutputDto>> GetList(GetCWGLAdvanceChargeListInput input)
        {
            var strflowid = input.FlowId.ToString();
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        let m = (from b in _advanceChargeDetailRepository.GetAll().Where(x => x.AdvanceChargeId == a.Id) select b).Sum(x => x.Money)
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                                             x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                                             x.ReceiveID == AbpSession.UserId.Value && x.Type != 6 && strflowid.GetFlowContainHideTask(x.Status))
                                         select c).Any()
                        select new CWGLAdvanceChargeListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Name = a.Name,
                            Cause = a.Cause,
                            Status = a.Status,
                            Money = a.Money,
                            SettleState = a.SettleState,
                            CreatorUserId = a.CreatorUserId,
                            AdvanceChargeMoney = m,
                            SettleState_Name = Enum.GetName(typeof(SettleState), a.SettleState),
                            OpenModel = openModel ? 1 : 2,
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(x => x.OpenModel).ThenByDescending(x => x.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);
                if (item.OpenModel == 2 && item.SettleState != 2 && item.CreatorUserId == AbpSession.UserId.Value)
                    item.OpenModel = 3;
            }
            return new PagedResultDto<CWGLAdvanceChargeListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<CWGLAdvanceChargeOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var m  = (from b in _advanceChargeDetailRepository.GetAll().Where(x => x.AdvanceChargeId == model.Id) select b).Sum(x => x.Money);
            var tmp = model.MapTo<CWGLAdvanceChargeOutputDto>();
            tmp.SettleState_Name = Enum.GetName(typeof(SettleState), model.SettleState);
            tmp.AdvanceChargeMoney = m;

            return tmp;
        }
        /// <summary>
        /// 添加一个CWGLAdvanceCharge
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateCWGLAdvanceChargeInput input)
        {
            var newmodel = new CWGLAdvanceCharge()
            {
                Name = input.Name,
                Cause = input.Cause,
                Money = input.Money
            };
            newmodel.Status = 0;
            await _repository.InsertAsync(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个CWGLAdvanceCharge
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateCWGLAdvanceChargeInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var logModel = new CWGLAdvanceCharge();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone<CWGLAdvanceCharge>();
                }
                dbmodel.Name = input.Name;
                dbmodel.Cause = input.Cause;
                dbmodel.Money = input.Money;
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
        private CWGLAdvanceChargeLogDto GetChangeModel(CWGLAdvanceCharge model)
        {
            var ret = model.MapTo<CWGLAdvanceChargeLogDto>();
            return ret;
        }
    }
}