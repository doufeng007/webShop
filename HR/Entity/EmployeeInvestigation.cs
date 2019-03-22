using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR.Entity
{
    /// <summary>
    /// 职前调查信息
    /// </summary>
    [Table("EmployeeInvestigation")]
    public class EmployeeInvestigation : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }

        public int? TenantId { get; set; }
    }
}
