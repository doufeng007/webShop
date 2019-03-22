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
    public interface IOAFixedAssetsScrapAppService : IApplicationService
    {

        
        Task<OAFixedAssetsScrapDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OAFixedAssetsScrapListDto>> GetAll(GetOAFixedAssetsScrapListInput input);


        Task<InitWorkFlowOutput> Create(OAFixedAssetsScrapCreateInput input);

        Task Update(OAFixedAssetsScrapUpdateInput input);

        
    }

}
