using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public class SearchProjectListStatus : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 0:待办 1:在办 2:已办
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public ProjectStatus? ProjectStatus { get; set; }
    }


    public class ProjectListStatus
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 送审单位
        /// </summary>
        public string SendUnitText { get; set; }
        public int SendUnit { get; set; }
        /// <summary>
        /// 送审金额
        /// </summary>
        public decimal? SendTotalBudget { get; set; }

        public int AppraisalTypeId { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string AppraisalType { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 预计完成时间
        /// </summary>
        public int? FinishDays { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// 项目负责人
        /// </summary>
        public string ProjectManagerUser { get; set; }

        public ProjectStatus? ProjectStatus { get; set; }

        public string ProjectStatusText { get; set; }

        /// <summary>
        /// 审定金额
        /// </summary>
        public decimal? AuditAmount { get; set; }
        /// <summary>
        /// 建安预算
        /// </summary>
        public decimal? SafaBudget { get; set; }
        public int Status { get; set; }
        public DateTime CreationTime { get; set; }

        public bool IsImportant { get; set; }

        public bool IsFollow { get; set; }

        /// <summary>
        /// 项目准备开始阶段
        /// </summary>
        public DateTime? ReadyStartTime { get; set; }
        /// <summary>
        /// 项目准备结束时间
        /// </summary>
        public DateTime? ReadyEndTime { get; set; }
    }
}
