using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public interface IOAPerformanceAppService : IApplicationService
    {
        InitWorkFlowOutput Create(OAPerformanceInputDto input);
        void Update(OAPerformanceInputDto input);

        OAPerformanceDto Get(GetWorkFlowTaskCommentInput input);


        PagedResultDto<OAPerformanceListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
