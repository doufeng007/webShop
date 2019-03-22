using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    [Table("EmployeeBusinessTripMember")]
    public class EmployeeBusinessTripMember : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public Guid BusinessTripId { get; set; }

        public long UserId { get; set; }

        public string Remark { get; set; }
    }
}
