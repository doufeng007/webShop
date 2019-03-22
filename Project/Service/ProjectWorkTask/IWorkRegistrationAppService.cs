using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project
{
    public interface IWorkRegistrationAppService : IApplicationService
    {
        Task<WorkRegistrationForViewOutput> GetForEdit(NullableIdDto<Guid> input);
        Task<WorkRegistrationOutput> Get(NullableIdDto<Guid> input);

        Task<WorkRegistrationForFlowOutput> GetForEditForFlow(GetWorkFlowTaskCommentInput input);


        Task<WorkRegistrationForViewPublishOutput> GetPublish(EntityDto<Guid> input);

        Task<WorkRegistrationForViewRelyOutput> GetRely(EntityDto<Guid> input);


        Task<List<WorkRegistrationList>> GetWorkRegPage(GetWorkRegistrationInput input);

        Task<PagedResultDto<PmOldProjectOutput>> GetWorkOldPmProject(GetPmProjectInput input);
        Task<PagedResultDto<PmProjectOutput>> GetWorkPmProject(GetPmProjectInput input);
        Task<PagedResultDto<WorkRegistrationList>> GetWorkRegistrationList(GetWorkRegistrationListInput input);
        Task<PagedResultDto<WorkRegistrationList>> GetWorkRegistrationOldList(GetWorkRegistrationListOldInput input);
    }

}
