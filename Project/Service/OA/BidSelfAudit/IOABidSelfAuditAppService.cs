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
    public interface IOABidSelfAuditAppService : IApplicationService
    {

        
        Task<OABidSelfAuditDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OABidSelfAuditListDto>> GetAll(GetOABidSelfAuditListInput input);


        Task<InitWorkFlowOutput> Create(OABidSelfAuditCreateInput input);

        Task Update(OABidSelfAuditUpdateInput input);

        
    }

}
