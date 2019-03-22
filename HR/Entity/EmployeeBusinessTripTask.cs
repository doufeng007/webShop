using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    [Table("EmployeeBusinessTripTask")]
    public class EmployeeBusinessTripTask : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }


        public Guid BusinessTripId { get; set; }

        public bool NotInPlan { get; set; }

        public string TaskName { get; set; }


        public int CompleteStatus { get; set; }


        public string Remark { get; set; }

    }
}
