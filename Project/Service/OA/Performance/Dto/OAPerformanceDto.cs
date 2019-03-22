using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OAPerformance))]
    public class OAPerformanceInputDto: CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public string Surname { get; set; }
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
        /// 上月计划
        /// </summary>
        public string PlanTask { get; set; }
        /// <summary>
        /// 完成计划
        /// </summary>
        public string FinishTask { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public decimal? FinishPersent { get; set; }
        /// <summary>
        /// 自我评价
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 领导评价
        /// </summary>
        public string LeaderComment { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 领导评分
        /// </summary>
        public int? Score { get; set; }
        public Guid? Main_id { get; set; }
    }
    [AutoMap(typeof(OAPerformance))]
    public class OAPerformanceDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Surname { get; set; }
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
        /// 上月计划
        /// </summary>
        public string PlanTask { get; set; }
        /// <summary>
        /// 完成计划
        /// </summary>
        public string FinishTask { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public decimal? FinishPersent { get; set; }
        /// <summary>
        /// 自我评价
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 领导评价
        /// </summary>
        public string LeaderComment { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 领导评分
        /// </summary>
        public int? Score { get; set; }
        public Guid? Main_id { get; set; }
    }

    [AutoMap(typeof(OAPerformance))]
    public class OAPerformanceListDto: BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public string Surname { get; set; }
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
        /// 上月计划
        /// </summary>
        public string PlanTask { get; set; }
        /// <summary>
        /// 完成计划
        /// </summary>
        public string FinishTask { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public decimal? FinishPersent { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
        /// <summary>
        /// 领导评分
        /// </summary>
        public int? Score { get; set; }
        public Guid? Main_id { get; set; }
    }
}
