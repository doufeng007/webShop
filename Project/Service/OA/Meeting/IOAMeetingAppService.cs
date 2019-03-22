using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IOAMeetingAppService : IApplicationService
    {

        
        Task<OAMeetingDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OAMeetingListDto>> GetAll(GetOAMeetingListInput input);


        Task<InitWorkFlowOutput> Create(OAMeetingCreateInput input);

        Task Update(OAMeetingUpdateInput input);

        
    }

}
