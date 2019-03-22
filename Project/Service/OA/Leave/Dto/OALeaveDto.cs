using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OALeave))]
    public class OALeaveInputDto: CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public int? Hours { get; set; }
        /// <summary>
        /// 外出原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }

        public string AuditUserText { get; set; }
    }
    [AutoMap(typeof(OALeave))]
    public class OALeaveDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public int? Hours { get; set; }
        /// <summary>
        /// 外出原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }

        public string AuditUserText { get; set; }
    }
    [AutoMap(typeof(OALeave))]
    public class OALeaveListDto: BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public int? Hours { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }

        public string AuditUserText { get; set; }
    }
}
