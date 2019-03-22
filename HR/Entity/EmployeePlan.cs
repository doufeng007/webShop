using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    public class EmployeePlan : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        /// <summary>
        /// 面试者
        /// </summary>
        public string ApplyUser { get; set; }
        /// <summary>
        /// 应聘部门
        /// </summary>
        public long? ApplyOrgId { get; set; }
        /// <summary>
        /// 应聘职位
        /// </summary>
        public Guid? ApplyPostId { get; set; }
        /// <summary>
        /// 应聘职位
        /// </summary>
        public string ApplyJob { get; set; }
        /// <summary>
        /// 应聘者联系方式
        /// </summary>
        public string Phone { get; set; }
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
        /// 汇总人
        /// </summary>
        public string MergeUserId { get; set; }
        /// <summary>
        /// 记录人
        /// </summary>
        public string RecordUserId { get; set; }
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
        /// 主管领导审批意见
        /// </summary>
        public string AdminVerifyDiscuss { get; set; }

        public int Status { get; set; }
        /// <summary>
        /// 是否入职
        /// </summary>
        public bool? IsJoin { get; set; }
        /// <summary>
        /// 入职备注
        /// </summary>
        public string JoinDes { get; set; }
    }

    public enum ApplyCount {
        /// <summary>
        /// 一面
        /// </summary>
        一面=1,
        /// <summary>
        /// 二面
        /// </summary>
        二面=2,
        /// <summary>
        /// 三面
        /// </summary>
        三面=3,
        /// <summary>
        /// 四面
        /// </summary>
        四面=4
    }

    public enum ApplyResult
    {
        /// <summary>
        /// 淘汰
        /// </summary>
        淘汰=0,
        /// <summary>
        /// 录用
        /// </summary>
        录用=1,
        /// <summary>
        /// 复试
        /// </summary>
        复试=2
    }
}
