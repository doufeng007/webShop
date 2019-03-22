using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply.Service
{
    public interface ISupplyRepairAppService : IApplicationService
    {
        Task<InitWorkFlowOutput> Create(CreateSupplyRepairDto input);

        /// <summary>
        /// 业务新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> CreateV2(CreateSupplyRepairDto input);

        Task Update(CreateSupplyRepairDto input);

        Task<SupplyRepairDto> Get(GetWorkFlowTaskCommentInput input);


        Task<SupplyRepairOutDto> GetForFlow(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<SupplyRepairDto>> GetAll(SupplyRepairListInputDtondSortedInputDto input);

        Task<PagedResultDto<SupplyRepairDto>> GetAllV2(SupplyRepairListInputDtondSortedInputDto input);

        Task<PagedResultDto<SupplyRepairDto>> GetMy(GetListInput input);


        Task Apply(Guid input);
        Task ApplyForFlow(Guid input);
        Task<ExecuteWorkFlowOutput> RepairSuccess(Guid input);

        Task<ExecuteWorkFlowOutput> RepairFailed(Guid input);

        [RemoteService(IsEnabled = false)]
        void AfterRepairAction(string instanceId, Guid flowId);

        [RemoteService(IsEnabled = false)]
        void RepairNoticeSupplierAction(string instanceId, string parameter);
    }
}
