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
    /// 报销申请
    /// </summary>
    [Table("OAFee")]
    public class OAFee : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }

        public string Contet { get; set; }

        public decimal? Money { get; set; }

        public DateTime? FeeDate { get; set; }
        public string AuditUser { get; set; }
        public int? Status { get; set; }
    }
}
