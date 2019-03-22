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
    public interface IOAFixedAssetsPurchaseAppService : IApplicationService
    {

        
        Task<OAFixedAssetsPurchaseDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OAFixedAssetsPurchaseListDto>> GetAll(GetOAFixedAssetsPurchaseListInput input);


        Task<InitWorkFlowOutput> Create(OAFixedAssetsPurchaseCreateInput input);

        Task Update(OAFixedAssetsPurchaseUpdateInput input);

        
    }

}
