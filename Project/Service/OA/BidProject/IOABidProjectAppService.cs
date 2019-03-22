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
    public interface IOABidProjectAppService : IApplicationService
    {

        
        Task<OABidProjectDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OABidProjectListDto>> GetAll(GetOABidProjectListInput input);


        Task<InitWorkFlowOutput> Create(OABidProjectCreateInput input);

        Task Update(OABidProjectUpdateInput input);

        void UpdateOABidProjectStatus(UpdateOABidProjectStatusInput input);
    }

}
