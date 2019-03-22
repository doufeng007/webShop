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
    /// 借款申请
    /// </summary>
    [Table("OABorrow")]
    public class OABorrow : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }

        public string Reason { get; set; }

        public decimal? Money { get; set; }

        public int BorrowDays { get; set; }

        public string AuditUser { get; set; }
        public int? Status { get; set; }
    }
}
