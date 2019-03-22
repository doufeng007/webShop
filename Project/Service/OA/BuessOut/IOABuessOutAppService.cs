using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public interface IOABuessOutAppService : IApplicationService
    {
        InitWorkFlowOutput Create(OABuessOutInputDto input);
        void Update(OABuessOutInputDto input);

        OABuessOutDto Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OABuessOutListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
