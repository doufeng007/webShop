using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Docment.Service
{
    public class DocmentDestroySearchDto: WorkFlowPagedAndSortedInputDto
    {
        public DocmentDestroyStatus? Status { get; set; }
    }
    public enum DocmentDestroyStatus {
        审核中 = 1,
        销毁中 = 2,
        驳回 = -2,
        已销毁 = -1
    }
}
