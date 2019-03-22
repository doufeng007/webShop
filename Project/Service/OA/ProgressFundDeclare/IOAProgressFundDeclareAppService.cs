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
    public interface IOAProgressFundDeclareAppService : IApplicationService
    {

        
        Task<OAProgressFundDeclareDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<OAProgressFundDeclareListDto>> GetAll(GetOAProgressFundDeclareListInput input);


        Task<InitWorkFlowOutput> Create(OAProgressFundDeclareCreateInput input);

        Task Update(OAProgressFundDeclareUpdateInput input);

        
    }

}
