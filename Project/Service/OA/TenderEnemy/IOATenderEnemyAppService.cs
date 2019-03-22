using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public interface IOATenderEnemyAppService: IApplicationService
    {
        InitWorkFlowOutput Create(OATenderEnemyInputDto input);
        void Update(OATenderEnemyInputDto input);

        OATenderEnemyDto Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OATenderEnemyListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
