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
    public interface IOATenderBuessAppService : IApplicationService
    {

        
        Task<OATenderBuessDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OATenderBuessListDto>> GetAll(GetOATenderBuessListInput input);


        Task<InitWorkFlowOutput> Create(OATenderBuessCreateInput input);

        Task Update(OATenderBuessUpdateInput input);

        
    }

}
