using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    [Table("EmployeeRegular")]
    public class EmployeeRegular : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public long UserId { get; set; }

        public Guid EmployeeId { get; set; }

        public DateTime ApplyDate { get; set; }

        public bool IsAdvanced { get; set; }


        public string WorkSummary { get; set; }



        public DateTime StrialBeginTime { get; set; }

        public DateTime StrialEndTime { get; set; }


        public string Remark { get; set; }


        public int Status { get; set; }


    }
}
