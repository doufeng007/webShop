using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    /// <summary>
    /// 员工教育经历
    /// </summary>
    [Table("EducationExperience")]
    public class EducationExperience : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public Guid EmployeeId { get; set; }
        public string SchoolName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Major { get; set; }
        public string Education { get; set; }

        public int? TenantId { get; set; }
    }
}
