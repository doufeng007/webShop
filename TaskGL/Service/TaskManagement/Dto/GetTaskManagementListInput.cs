using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskGL.Enum;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;

namespace TaskGL
{
    public class GetTaskManagementListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }


        public TaskManagementStateEnum? TaskStatus { get; set; }


        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskManagementTypeEnum? Type { get; set; }


        public bool? IsFollow { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
