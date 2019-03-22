using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMap(typeof(EmployeePlan))]
    public class EmployeePlanAdminResultInput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 领导意见
        /// </summary>
        public string VerifyDiscuss { get; set; }
        /// <summary>
        /// 面试结果
        /// </summary>
        public ApplyResult Result { get; set; }
    }
}
