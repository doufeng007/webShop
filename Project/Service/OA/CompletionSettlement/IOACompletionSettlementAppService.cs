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
    public interface IOACompletionSettlementAppService : IApplicationService
    {

        
        Task<OACompletionSettlementDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OACompletionSettlementListDto>> GetAll(GetOACompletionSettlementListInput input);


        Task<InitWorkFlowOutput> Create(OACompletionSettlementCreateInput input);

        Task Update(OACompletionSettlementUpdateInput input);

        
    }

}
