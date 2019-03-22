using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    /// <summary>
    /// 人事安排面试时间和面试官
    /// </summary>
    [AutoMap(typeof(EmployeePlan))]
    public  class EmployeePlanInput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 预约面试时间
        /// </summary>
        public DateTime? ApplyTime { get; set; }
        /// <summary>
        /// 面试官
        /// </summary>
        public string EmployeeUserIds { get; set; }
        /// <summary>
        /// 汇总人
        /// </summary>
        public string MergeUserId { get; set; }
        /// <summary>
        /// 记录人
        /// </summary>
        public string RecordUserId { get; set; }
        /// <summary>
        /// 面试轮数
        /// </summary>
        public ApplyCount ApplyCount { get; set; }
    }
}
