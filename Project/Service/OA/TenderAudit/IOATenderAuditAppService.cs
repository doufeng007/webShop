using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public interface IOATenderAuditAppService: IApplicationService
    {
        Task<InitWorkFlowOutput> Create(OATenderAuditInputDto input);
        Task Update(OATenderAuditInputDto input);

        Task<OATenderAuditDto> Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OATenderAuditListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
