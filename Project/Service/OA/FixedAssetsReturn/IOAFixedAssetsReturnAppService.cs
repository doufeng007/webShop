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
    public interface IOAFixedAssetsReturnAppService : IApplicationService
    {

        
        Task<OAFixedAssetsReturnDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OAFixedAssetsReturnListDto>> GetAll(GetOAFixedAssetsReturnListInput input);


        Task<InitWorkFlowOutput> Create(OAFixedAssetsReturnCreateInput input);

        Task Update(OAFixedAssetsReturnUpdateInput input);

        
    }

}
