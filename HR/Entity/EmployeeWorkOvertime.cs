using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    [Table("EmployeeWorkOvertime")]
    public class EmployeeWorkOvertime : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public long UserId { get; set; }

        public DateTime ApplyDate { get; set; }


        public string Reason { get; set; }

        public int PreHours { get; set; }

        public int? Hours { get; set; }

        public int Status { get; set; }


        public string Remark { get; set; }




    }
}
