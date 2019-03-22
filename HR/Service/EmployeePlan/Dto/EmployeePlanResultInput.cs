using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    /// <summary>
    /// 分管领导审核面试结果
    /// </summary>
    [AutoMap(typeof(EmployeePlan))]
    public class EmployeePlanResultInput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 分管领导意见
        /// </summary>
        public string VerifyDiscuss { get; set; }
        /// <summary>
        /// 是否需要主管领导审核
        /// </summary>
        public bool? NeedAdmin { get; set; }
        /// <summary>
        /// 主管领导
        /// </summary>
        public string AdminUserId { get; set; }
        /// <summary>
        /// 面试结果
        /// </summary>
        public ApplyResult? Result { get; set; }
    }
}
