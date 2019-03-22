using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR.Entity
{
    [Table("InterviewResult")]
    public class InterviewResult : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string JobName { get; set; }
        public DateTime Birthday { get; set; }
        public int WorkYear { get; set; }
        public bool IsLater { get; set; }
        public int Achievement { get; set; }
        public DateTime PreWorkDate { get; set; }
        public int PreSalary { get; set; }
        public int FirstAchievement { get; set; }
        public string FirstSuggest { get; set; }
        public string FirstComment { get; set; }
        public int SecondAchievement { get; set; }
        public string SecondSuggest { get; set; }
        public string SecondComment { get; set; }
        public int? TenantId { get; set; }
    }
}
