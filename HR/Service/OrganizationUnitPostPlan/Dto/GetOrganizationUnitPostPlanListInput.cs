using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    public class GetOrganizationUnitPostPlanListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {

        /// <summary>
        /// 1 新增编制 2 编制整改
        /// </summary>
        public int ActionType { get; set; } = 1;

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
