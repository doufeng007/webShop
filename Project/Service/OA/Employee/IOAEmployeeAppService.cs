using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public interface IOAEmployeeAppService : IApplicationService
    {
        InitWorkFlowOutput Create(OAEmployeeInputDto input);
        void Update(OAEmployeeInputDto input);

        OAEmployeeDto Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OAEmployeeListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
