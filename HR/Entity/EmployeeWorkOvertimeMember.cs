using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    [Table("EmployeeWorkOvertimeMember")]
    public class EmployeeWorkOvertimeMember : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public Guid WorkOvertimeId { get; set; }

        public long UserId { get; set; }

        public string Remark { get; set; }
    }
}
