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
    /// 用车管理
    /// </summary>
    [Table("OAUseCar")]
    public class OAUseCar : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }
        public int? Status { get; set; }

        public string AuditUser { get; set; }

        public string Reason { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int Person { get; set; }

        public string Destination { get; set; }

        public decimal? Mileage { get; set; }
    }
}
