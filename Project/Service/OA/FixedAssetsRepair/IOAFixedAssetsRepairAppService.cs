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
    public interface IOAFixedAssetsRepairAppService : IApplicationService
    {

        
        Task<OAFixedAssetsRepairDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OAFixedAssetsRepairListDto>> GetAll(GetOAFixedAssetsRepairListInput input);


        Task<InitWorkFlowOutput> Create(OAFixedAssetsRepairCreateInput input);

        Task Update(OAFixedAssetsRepairUpdateInput input);

        Task BeginRepairOAFA(Guid fAId);
    }

}
