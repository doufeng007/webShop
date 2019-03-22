using Abp.AutoMapper;
using Abp.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    /// <summary>
    /// 登记员、汇总员填写面试结果
    /// </summary>
    [AutoMap(typeof(EmployeePlan))]
    public  class EmployeePlanCommentInput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 面试记录
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 协商意见
        /// </summary>
        public string Discuss { get; set; }
        /// <summary>
        /// 面试官
        /// </summary>
        public string EmployeeUserIds { get; set; }
        /// <summary>
        /// 汇总人
        /// </summary>
        public string MergeUserId { get; set; }

        /// <summary>
        /// 分管领导
        /// </summary>
        public string VerifyUserId { get; set; }
        /// <summary>
        /// 面试结果附件
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }
}
