using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public interface IOAFeeAppService : IApplicationService
    {
        InitWorkFlowOutput Create(OAFeeInputDto input);
        void Update(OAFeeInputDto input);

        OAFeeDto Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OAFeeListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
