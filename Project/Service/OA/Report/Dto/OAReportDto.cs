using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project.Service.OA.Report.Dto
{
    [AutoMap(typeof(OAReport))]
    public class OAReportInputDto : CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime? StarTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        [Required]
        public ReportType ReportType { get; set; }
        [Required]
        public string Content { get; set; }

        public string ReportAudits { get; set; }
        public string ReportAuditsText { get; set; }
        public int? Status { get; set; }

        public string UserName { get; set; }
    }
    [AutoMap(typeof(OAReport))]
    public class OAReportDto : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime? StarTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        [Required]
        public ReportType ReportType { get; set; }
        [Required]
        public string Content { get; set; }

        public string ReportAudits { get; set; }
        public string ReportAuditsText { get; set; }

        public string UserName { get; set; }

    }

    [AutoMap(typeof(OAReport))]
    public class OAReportListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime? StarTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        [Required]
        public ReportType ReportType { get; set; }
        [Required]
        public string Content { get; set; }

        public string ReportAudits { get; set; }
        public string ReportAuditsText { get; set; }

        public string UserName { get; set; }

    }
    public class SearchReportInput : WorkFlowPagedAndSortedInputDto
    {
        public ReportWay? ReportWay { get; set; }

        public ReportType? ReportType { get; set; }

        public string Key { get; set; }
    }

    public enum ReportWay
    {
        我写的 = 0,
        收到的 = 1
    }
}
