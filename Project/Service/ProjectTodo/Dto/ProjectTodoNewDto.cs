using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public class ProjectTodoNewDto
    {
        /// <summary>
        ///待办项目数量
        /// </summary>
        public int TodoCount { get; set; }
        /// <summary>
        /// 在办项目数量
        /// </summary>
        public int DoingCount { get; set; }
        /// <summary>
        /// 已办项目数量
        /// </summary>
        public int DoneCount { get; set; }
        /// <summary>
        /// 在办项目金额
        /// </summary>
        public decimal DoingSum { get; set; }
        /// <summary>
        /// 已办项目金额
        /// </summary>
        public decimal DoneSum { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public decimal HandleRate { get; set; }
    }
    [AutoMap(typeof(WorkFlowTask))]
    public class ProjectTodoNewListDto : EntityDto<Guid>
    {
        public Guid FlowID { get; set; }
        public string FlowName { get; set; }
        public Guid StepID { get; set; }
        public string StepName { get; set; }
        public string InstanceID { get; set; }
        public Guid GroupID { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public long SenderID { get; set; }
        public string SenderName { get; set; }

        public DateTime SenderTime { get; set; }
        public long ReceiveID { get; set; }
        public string ReceiveName { get; set; }
        public DateTime ReceiveTime { get; set; }
        public int Status { get; set; }
        public string StatusTitle { get; set; }
        public int? TodoType { get; set; }
        /// <summary>
        /// 是否重点项目
        /// </summary>
        public bool IsImportent { get; set; }
        /// <summary>
        /// 是否关注
        /// </summary>
        public bool IsFollow { get; set; }
        public int? AppraisalTypeId { get; set; }
    }
    public class SearchTodoInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 0:流程待办 1:我的待办
        /// </summary>
        public int TodoType { get; set; }

        /// <summary>
        /// 1:项目类 2:oa类
        /// </summary>
        public int? FlowType { get; set; }

        /// <summary>
        /// 待办状态 0:待办 1:在办 2:已办 
        /// </summary>
        public int Status { get; set; }
    }


    /// <summary>
    /// 日常待办事项统计表
    /// </summary>
    public class OATodoList
    {
        public string Title { get; set; }
        /// <summary>
        /// 紧急程度
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public string WatchUser { get; set; }
        /// <summary>
        /// 办事人
        /// </summary>
        public string DoUser { get; set; }
        /// <summary>
        /// 接受时间
        /// </summary>
        public DateTime ReciveTime { get; set; }
        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// 是否在办
        /// </summary>
        public bool Doing { get; set; }

        public string Des { get; set; }
    }

    public class TodoCountDtoForSql
    {
        public int ProjectTodoCount { get; set; }

        public int OACount { get; set; }
    }

    public class GetProjectTodoCountOutput : TodoCountDtoForSql
    {
        public Guid Id { get; set; }
    }

}
