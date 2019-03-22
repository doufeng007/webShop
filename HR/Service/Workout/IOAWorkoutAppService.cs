using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;
using System.Threading.Tasks;

namespace HR
{
    public interface IOAWorkoutAppService: IApplicationService
    {
        Task<InitWorkFlowOutput> Create(OAWorkoutInputDto input);
        Task Update(OAWorkoutInputDto input);


        Task<OAWorkoutDto> Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OAWorkoutListDto> GetAll(GetOAWorkoutListInput input);
       List<OAWorkoutListByCarDto> GetList(GetOAWorkoutListByCarInput input);
       void WorkoutRelationUserId(Guid instanceID);
    }
}
