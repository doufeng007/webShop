using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace HR
{
    [AutoMap(typeof(EmployeePlan))]
    public class CreatePlanInput: CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        /// <summary>
        /// 面试者
        /// </summary>
        public string ApplyUser { get; set; }
        ///// <summary>
        ///// 应聘部门
        ///// </summary>
        //public long ApplyOrgId { get; set; }
        /// <summary>
        /// 应聘职位
        /// </summary>
        public Guid ApplyPostId { get; set; }
        /// <summary>
        /// 应聘职位
        /// </summary>
        public string ApplyJob { get; set; }
        /// <summary>
        /// 应聘者联系方式
        /// </summary>
        public string Phone { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }
    [AutoMap(typeof(EmployeePlan))]
    public class EmployeePlanEditInput: CreateWorkFlowInstance
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 面试者
        /// </summary>
        [LogColumn("面试者", true)]
        public string ApplyUser { get; set; }
        /// <summary>
        /// 应聘者联系方式
        /// </summary>
        [LogColumn("应聘者联系方式", true)]
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
        [LogColumn("应聘职位", true)]
        public Guid? ApplyPostId { get; set; }
        /// <summary>
        /// 应聘职位
        /// </summary>
        [LogColumn("应聘职位", true)]
        public string ApplyJob { get; set; }


        /// <summary>
        /// 预约面试时间
        /// </summary>
        [LogColumn("预约面试时间", true)]
        public DateTime? ApplyTime { get; set; }
        /// <summary>
        /// 面试官
        /// </summary>
        public string EmployeeUserIds { get; set; }
        /// <summary>
        /// 面试官姓名
        /// </summary>
        [LogColumn("面试官", true)]
        public string EmployeeUserIds_Name { get; set; }
        /// <summary>
        /// 汇总人
        /// </summary>
        public string MergeUserId { get; set; }

        /// <summary>
        /// 汇总人姓名
        /// </summary>
        [LogColumn("汇总人", true)]
        public string MergeUserId_Name { get; set; }
        /// <summary>
        /// 记录人
        /// </summary>
        public string RecordUserId { get; set; }
        /// <summary>
        /// 记录人姓名
        /// </summary>
        [LogColumn("记录人", true)]
        public string RecordUserId_Name { get; set; }
        /// <summary>
        /// 面试记录
        /// </summary>
        [LogColumn("面试记录", true)]
        public string Comment { get; set; }
        /// <summary>
        /// 协商意见
        /// </summary>
        [LogColumn("协商意见", true)]
        public string Discuss { get; set; }
        /// <summary>
        /// 分管领导意见
        /// </summary>
        [LogColumn("分管领导意见", true)]
        public string VerifyDiscuss { get; set; }
        /// <summary>
        /// 分管领导
        /// </summary>
        public string VerifyUserId { get; set; }
        /// <summary>
        /// 分管领导
        /// </summary>
        [LogColumn("分管领导", true)]
        public string VerifyUserId_Name { get; set; }
        /// <summary>
        /// 是否需要主管领导审核
        /// </summary>
        [LogColumn("是否需要主管领导审核", true)]
        public bool? NeedAdmin { get; set; }
        /// <summary>
        /// 面试结果
        /// </summary>
        [LogColumn("面试结果", true)]
        public ApplyResult? Result { get; set; }
        /// <summary>
        /// 主管领导
        /// </summary>
        public string AdminUserId { get; set; }
        /// <summary>
        /// 主管领导姓名
        /// </summary>
        [LogColumn("主管领导", true)]
        public string AdminUserId_Name { get; set; }
        /// <summary>
        /// 主管领导审批意见
        /// </summary>
        [LogColumn("主管领导审批意见", true)]
        public string AdminVerifyDiscuss { get; set; }
        /// <summary>
        /// 面试简历
        /// </summary>
        [LogColumn("面试简历", true)]
        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        /// <summary>
        /// 面试结果
        /// </summary>
        [LogColumn("面试结果", true)]
        public List<GetAbpFilesOutput> ResultFileList { get; set; } = new List<GetAbpFilesOutput>();
        /// <summary>
        /// 是否入职
        /// </summary>
        [LogColumn("是否入职", true)]
        public bool? IsJoin { get; set; }
        /// <summary>
        /// 入职备注
        /// </summary>
        [LogColumn("入职备注", true)]
        public string JoinDes { get; set; }

        public bool IsUpdateForChange { get; set; }
    }
}
