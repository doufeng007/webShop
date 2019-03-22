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
    public interface IOAFixedAssetsAppService : IApplicationService
    {

        
        Task<OAFixedAssetsDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OAFixedAssetsListDto>> GetAll(GetOAFixedAssetsListInput input);


        Task<InitWorkFlowOutput> Create(OAFixedAssetsCreateInput input);

        Task Update(OAFixedAssetsUpdateInput input);


        void UpdateOAFixedAssetsStatusAsync(UpdateOAFixedAssetsStatusInput input);

    }

}
