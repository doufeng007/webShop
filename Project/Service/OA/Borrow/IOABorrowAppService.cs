using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project.Service.OA.Borrow
{
    public interface  IOABorrowAppService: IApplicationService
    {
        InitWorkFlowOutput Create(OABorrowInputDto input);
        void Update(OABorrowInputDto input);

        OABorrowDto Get(GetWorkFlowTaskCommentInput input);

        PagedResultDto<OABorrowListDto> GetAll(WorkFlowPagedAndSortedInputDto input);
    }
}
