using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Docment.Service
{
    public class DocmentMoveSearchDto : WorkFlowPagedAndSortedInputDto
    {
        public MoveStatus? Status { get; set; }

    }

    public enum MoveStatus {
        审核中=1,
        移交中=2,
        驳回=-2,
        已移交=-1
    }
}
