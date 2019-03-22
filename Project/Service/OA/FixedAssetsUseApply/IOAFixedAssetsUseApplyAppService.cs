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
    public interface IOAFixedAssetsUseApplyAppService : IApplicationService
    {

        
        Task<OAFixedAssetsUseApplyDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OAFixedAssetsUseApplyListDto>> GetAll(GetOAFixedAssetsUseApplyListInput input);


        Task<InitWorkFlowOutput> Create(OAFixedAssetsUseApplyCreateInput input);

        Task Update(OAFixedAssetsUseApplyUpdateInput input);

        
    }

}
