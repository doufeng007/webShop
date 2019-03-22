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
    public interface IOABidFilePurchaseAppService : IApplicationService
    {

        
        Task<OABidFilePurchaseDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OABidFilePurchaseListDto>> GetAll(GetOABidFilePurchaseListInput input);


        Task<InitWorkFlowOutput> Create(OABidFilePurchaseCreateInput input);

        Task Update(OABidFilePurchaseUpdateInput input);

        
    }

}
