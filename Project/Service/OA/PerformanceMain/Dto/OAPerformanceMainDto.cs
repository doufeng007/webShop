using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OAPerformanceMain))]
    public class OAPerformanceMainInputDto : CreateWorkFlowInstance
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
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 参与员工
        /// </summary>
        public string AuditUser { get; set; }

        public string Des { get; set; }

        public string AuditUserText { get; set; }

        public List<OAPerformanceDto> OAPerformance { get; set; }
    }
    [AutoMap(typeof(OAPerformanceMain))]
    public class OAPerformanceMainDto: WorkFlowTaskCommentResult
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
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 参与员工
        /// </summary>
        public string AuditUser { get; set; }

        public string Des { get; set; }

        public string AuditUserText { get; set; }

        public List<OAPerformanceDto> OAPerformance { get; set; }
    }
    [AutoMap(typeof(OAPerformanceMain))]
    public class OAPerformanceMainListDto: BusinessWorkFlowListOutput
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

        public string AuditUserText { get; set; }
    }


}
