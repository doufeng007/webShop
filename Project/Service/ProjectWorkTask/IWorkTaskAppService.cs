using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Threading.Tasks;

namespace Project
{
    public interface IWorkTaskAppService : IApplicationService
    {
        Task WriteWorkLog(WorkLogInput input);

        Task<InitWorkFlowOutput> WriteRegistration(WorkRegistrationInput input);

        Task UpdateRegistration(UpdateWorkRegistrationInput input);



        [RemoteService(false)]
        string ProjectWriteRegisFlowActive(Guid instanceId);

        PagedResultDto<ProjectWorkTaskList> GetWorkTaskPage(GetWorkTaskListInput input);
        Task<PmInitWorkFlowOutput> WriteRegistrationIsPm(WorkRegistrationInput input);

        //Task<PagedResultDto<WorkAuditList>> GetWorkAuditPage(GetWorkAuditListInput input);


        //Task WriteWorkInformationEnter(WorkInformationEnterInput input);

        //Tuple<string, bool> GetStepComment(WorkInformationEnterInput input);
    }

}