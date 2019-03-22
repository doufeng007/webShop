using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    [Table("EmployeeSkill")]
    public class EmployeeSkill : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public Guid EmployeeId { get; set; }
    }
}
