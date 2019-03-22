using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Project
{
    /// <summary>
    /// 工作汇报
    /// </summary>
    [Table("OAReport")]
    public class OAReport : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }

        public DateTime? StarTime { get; set; }

        public DateTime? EndTime { get; set; }

        public ReportType ReportType { get; set; }

        public string Content { get; set; }

        public string ReportAudits { get; set; }

        public int? Status { get; set; }
    }

    public enum ReportType
    {
        日报 = 0,
        周报 = 1,
        月报 = 2,
        季报 = 3,
        年报 = 4,
        自定义 = 5
    }
}
