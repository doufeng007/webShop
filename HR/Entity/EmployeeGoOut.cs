using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    [Table("EmployeeGoOut")]
    public class EmployeeGoOut : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public long UserId { get; set; }


        public DateTime GoOutTime { get; set; }

        public int GoOutHour { get; set; }

        public DateTime BackTime { get; set; }

        public string OutTele { get; set; }

        public string Reason { get; set; }

        public string Remark { get; set; }

        public int Status { get; set; }

    }
}
