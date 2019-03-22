using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Abp.WorkFlow
{
    public interface IWorkFlowAppService : IApplicationService
    {

        Task<Guid> CreateWorkFlow(CreateWorkFlowInput input);

        Task UpdateWorkFlow(CreateWorkFlowInput input);

        Task DeleteWorkFlow(EntityDto<Guid> input);


        Task<GetWorkFlowUrlParameterOutput> GetWorkFlowUrlParameterAsync(GetWorkFlowUrlParameterInput input);

        Task<GetNextStepForRunOutput> GetNextStepForRun(GetNextStepForRunInput input);
        GetNextStepForRunOutput GetNextStepForRunSync(GetNextStepForRunInput input);


        Task<GetBackStepsForRunOutput> GetBackStepsForRun(GetBackStepsForRunInput input);

        Task<EidtWorkFlowOutput> GetForEdit(EntityDto<Guid> input);


        Task<EidtWorkFlowOutput> GetForEditByVersionNum(GetForEditByVersionNumInput input);


        Task<PagedResultDto<GetWorkFlowListDto>> GetList(GetWorkFlowListInput input);

        Task<List<GetWorkFlowInstanceStatusOutput>> GetStatussAsync(WorkFlowBaseInput input);

        List<WorkFlowTask> GetTaskList(string instanceId, Guid flowID, Guid? groupID);
        string GetWorkFlowOut(Guid flowID);
        void GetWorkFlowIn(Guid fileId, bool isNew, string extName);
        void GetWorkFlowCopy(Guid flowID);
        Task DeleteFlowFirstData(DeleteFlowFirstStepDataIn input);
    }

}
