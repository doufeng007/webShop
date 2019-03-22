using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    /// <summary>
    /// 员工工作经历
    /// </summary>
    [Table("WorkExperience")]
    public class WorkExperience : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public Guid EmployeeId { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string Job { get; set; }
        public string Salary { get; set; }
        public string Reason { get; set; }
        public string ManagerName { get; set; }
        public string ManagerPhone { get; set; }
        public int? TenantId { get; set; }

        public string DepartmentName { get; set; }
    }
}
