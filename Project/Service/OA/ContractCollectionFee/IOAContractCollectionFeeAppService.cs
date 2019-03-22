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
    public interface IOAContractCollectionFeeAppService : IApplicationService
    {

        
        Task<OAContractCollectionFeeDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OAContractCollectionFeeListDto>> GetAll(GetOAContractCollectionFeeListInput input);


        Task<InitWorkFlowOutput> Create(OAContractCollectionFeeCreateInput input);

        Task Update(OAContractCollectionFeeUpdateInput input);

        
    }

}
