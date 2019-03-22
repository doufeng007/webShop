using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
   public interface IOACustomerAppService: IApplicationService
    {
        InitWorkFlowOutput Create(OACustomerInputDto input);
        void Update(OACustomerInputDto input);

        OACustomerDto Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OACustomerListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
