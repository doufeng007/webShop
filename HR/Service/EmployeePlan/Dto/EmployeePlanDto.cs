using Abp.AutoMapper;
using Abp.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMap(typeof(EmployeePlan),typeof(EmployeeResult))]
    public class EmployeePlanDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 面试者
        /// </summary>
        public string ApplyUser { get; set; }
        /// <summary>
        /// 应聘者联系方式
        /// </summary>
        public string Phone { get; set; }
        ///// <summary>
        ///// 应聘部门
        ///// </summary>
        //public long? ApplyOrgId { get; set; }
        ///// <summary>
        ///// 应聘部门
        ///// </summary>
        //public string ApplyOrgName { get; set; }
        /// <summary>
        /// 应聘职位
        /// </summary>
        public Guid? ApplyPostId { get; set; }
        ///// <summary>
        ///// 应聘职位
        ///// </summary>
        //public string ApplyPostName { get; set; }
        /// <summary>
        /// 应聘职位
        /// </summary>
        public string ApplyJob { get; set; }
        /// <summary>
        /// 面试编号
        /// </summary>
        public string ApplyNo { get; set; }
        /// <summary>
        /// 面试轮数
        /// </summary>
        public ApplyCount ApplyCount { get; set; }
        /// <summary>
        /// 预约面试时间
        /// </summary>
        public DateTime? ApplyTime { get; set; }
        /// <summary>
        /// 面试官
        /// </summary>
        public string EmployeeUserIds { get; set; }
        /// <summary>
        /// 面试官姓名
        /// </summary>
        public string EmployeeUserIds_Name { get; set; }
        /// <summary>
        /// 汇总人
        /// </summary>
        public string MergeUserId { get; set; }

        /// <summary>
        /// 汇总人姓名
        /// </summary>
        public string MergeUserId_Name { get; set; }
        /// <summary>
        /// 记录人
        /// </summary>
        public string RecordUserId { get; set; }
        /// <summary>
        /// 记录人姓名
        /// </summary>
        public string RecordUserId_Name { get; set; }
        /// <summary>
        /// 面试记录
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 协商意见
        /// </summary>
        public string Discuss { get; set; }
        /// <summary>
        /// 分管领导意见
        /// </summary>
        public string VerifyDiscuss { get; set; }
        /// <summary>
        /// 分管领导
        /// </summary>
        public string VerifyUserId { get; set; }
        /// <summary>
        /// 分管领导
        /// </summary>
        public string VerifyUserId_Name { get; set; }
        /// <summary>
        /// 是否需要主管领导审核
        /// </summary>
        public bool? NeedAdmin { get; set; }
        /// <summary>
        /// 面试结果
        /// </summary>
        public ApplyResult? Result { get; set; }
        /// <summary>
        /// 主管领导
        /// </summary>
        public string AdminUserId { get; set; }
        /// <summary>
        /// 主管领导姓名
        /// </summary>
        public string AdminUserId_Name { get; set; }
        /// <summary>
        /// 主管领导审批意见
        /// </summary>
        public string AdminVerifyDiscuss { get; set; }
        /// <summary>
        /// 面试简历
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        /// <summary>
        /// 面试结果
        /// </summary>
        public List<GetAbpFilesOutput> ResultFileList { get; set; } = new List<GetAbpFilesOutput>();
        /// <summary>
        /// 面试记录
        /// </summary>
        public List<EmployeePlanDto> Log { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 是否入职
        /// </summary>
        public bool? IsJoin { get; set; }
        /// <summary>
        /// 入职备注
        /// </summary>
        public string JoinDes { get; set; }

        /// <summary>
        /// 面试计划
        /// </summary>
        public Guid? EmployeePlanId { get; set; }
    }
}
