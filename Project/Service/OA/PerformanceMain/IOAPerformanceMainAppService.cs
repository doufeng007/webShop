using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public interface IOAPerformanceMainAppService : IApplicationService
    {
        InitWorkFlowOutput Create(OAPerformanceMainInputDto input);
        void Update(OAPerformanceMainInputDto input);

        OAPerformanceMainDto Get(GetWorkFlowTaskCommentInput input);


        PagedResultDto<OAPerformanceMainListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
