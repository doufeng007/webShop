using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace  ZCYX.FRMSCore.Application
{
    public class GetTaskManagementRelationListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 流程编号
        /// </summary>
        public Guid FlowId { get; set; }

        /// <summary>
        /// 关联编号
        /// </summary>
        public Guid InStanceId { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        public Guid TaskManagementId { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }

    public class GetTaskManagementRelationInput
    {
        /// <summary>
        /// 流程编号
        /// </summary>
        public Guid FlowId { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        public Guid RelationTaskId { get; set; }
    }
}
