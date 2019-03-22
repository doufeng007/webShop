using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public interface IOALeaveAppService: IApplicationService
    {
        InitWorkFlowOutput Create(OALeaveInputDto input);
        void Update(OALeaveInputDto input);

        OALeaveDto Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OALeaveListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
