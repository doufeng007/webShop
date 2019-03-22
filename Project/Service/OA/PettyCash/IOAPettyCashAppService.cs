using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public interface IOAPettyCashAppService : IApplicationService
    {
        InitWorkFlowOutput Create(OAPettyCashInputDto input);
        void Update(OAPettyCashInputDto input);

        OAPettyCashDto Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OAPettyCashListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
