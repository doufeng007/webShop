using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [Table("OAPettyCash")]
    public class OAPettyCash : FullAuditedEntity<Guid>
    {
        public string SignUper { get; set; }
        public string Title { get; set; }
        public decimal? Money { get; set; }

        public string Reason { get; set; }
        public string AuditUser { get; set; }
        public int? Status { get; set; }
    }
}
