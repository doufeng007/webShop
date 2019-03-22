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
    public interface IOAWorkonAppService: IApplicationService
    {
        Task<InitWorkFlowOutput> Create(OAWorkonInputDto input);
        Task Update(OAWorkonInputDto input);


        Task<OAWorkonDto> Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OAWorkonListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
