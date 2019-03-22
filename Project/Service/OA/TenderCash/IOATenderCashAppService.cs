using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project.Service.OA.TenderCash
{
    public interface IOATenderCashAppService: IApplicationService
    {
        Task<InitWorkFlowOutput> Create(OATenderCashInputDto input);
        Task Update(OATenderCashInputDto input);

        Task<OATenderCashDto> Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OATenderCashListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
