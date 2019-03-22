using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public interface IOAUseCarAppService: IApplicationService
    {
        InitWorkFlowOutput Create(OAUseCarInputDto input);
        void Update(OAUseCarInputDto input);

        OAUseCarDto Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OAUseCarListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
