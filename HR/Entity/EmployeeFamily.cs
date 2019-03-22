using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    /// <summary>
    /// 员工家庭情况
    /// </summary>
    [Table("EmployeeFamily")]
    public class EmployeeFamily : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public int Age { get; set; }
        public string Company { get; set; }

        public string Job { get; set; }
        public string Phone { get; set; }
        public int? TenantId { get; set; }
    }
}
