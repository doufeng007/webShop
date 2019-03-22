using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR.Entity
{
    [Table("EmployeeSalary")]
    public class EmployeeSalary : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public Guid EmployeeId { get; set; }
        public long Department { get; set; }
        public string Job { get; set; }
        public string Name { get; set; }
        public string Salary { get; set; }
        public decimal PreSalary { get; set; }
        public int? Communicate { get; set; }
        public int? Traffic { get; set; }
        public int? Computer { get; set; }
        public string Period { get; set; }
        public DateTime EntryTime { get; set; }
        public string Des { get; set; }
        public int? TenantId { get; set; }
    }
}
