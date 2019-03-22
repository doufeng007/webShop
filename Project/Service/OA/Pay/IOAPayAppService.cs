using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project.Service.OA.Pay
{
    public interface IOAPayAppService: IApplicationService
    {
        InitWorkFlowOutput Create(OAPayInputDto input);
        void Update(OAPayInputDto input);

        OAPayDto Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OAPayListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
