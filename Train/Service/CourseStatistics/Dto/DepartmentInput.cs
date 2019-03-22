using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public class DepartmentInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 部门编号
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// 是否是部门领导
        /// </summary>
        public bool? IsLeader { get; set; }

        /// <summary>
        /// 入职时间-起
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 入职时间-止
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 学员姓名
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 统计时间-年
        /// </summary>
        public int StatisticYear { get; set; }

        /// <summary>
        /// 统计时间-月
        /// </summary>
        public int StatisticMonth { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
