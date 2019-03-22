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
    public interface IOABidProjectCheckAppService : IApplicationService
    {

        
        Task<OABidProjectCheckDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OABidProjectCheckListDto>> GetAll(GetOABidProjectCheckListInput input);


        Task<InitWorkFlowOutput> Create(OABidProjectCheckCreateInput input);

        Task Update(OABidProjectCheckUpdateInput input);

        
    }

}
