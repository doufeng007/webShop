using Abp.Runtime.Validation;
using CWGL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace CWGL
{
    public class GetCW_PersonalTodoListInput : PagedAndSortedInputDto, IShouldNormalize
    {

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        public Guid? FlowId { get; set; }

        /// <summary>
        /// 1 收款 2付款
        /// </summary>
        public RefundActionType? ActionType { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
