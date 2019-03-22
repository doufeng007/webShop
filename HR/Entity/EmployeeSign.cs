using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    [Table("EmployeeSign")]
    public class EmployeeSign : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public long UserId { get; set; }


        public DateTime? GoToWorkTime { get; set; }

        public DateTime? GoOfWork { get; set; }

        public DateTime MakeDate { get; set; }

        public string Remark { get; set; }

        public int Status { get; set; }

    }
}
