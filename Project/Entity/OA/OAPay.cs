using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// 付款申请
    /// </summary>
    [Table("OAPay")]
    public class OAPay : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }

        public string ToCompanyName { get; set; }

        public string ProjectName { get; set; }

        public string No { get; set; }

        public decimal? Money { get; set; }

        public string Content { get; set; }

        public string AuditUser { get; set; }
        public int? Status { get; set; }
    }
}
