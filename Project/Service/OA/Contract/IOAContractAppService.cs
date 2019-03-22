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
    public interface IOAContractAppService : IApplicationService
    {

        
        Task<OAContractDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OAContractListDto>> GetAll(GetOAContractListInput input);


        Task<InitWorkFlowOutput> Create(OAContractCreateInput input);

        Task Update(OAContractUpdateInput input);

        
    }

}
