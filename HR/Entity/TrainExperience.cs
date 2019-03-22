using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    /// <summary>
    /// 员工培训经历
    /// </summary>
    [Table("TrainExperience")]
    public class TrainExperience : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public Guid EmployeeId { get; set; }
        public string TrainName { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public string Classes { get; set; }
        public string Certificate { get; set; }

        public int? TenantId { get; set; }
    }
}
