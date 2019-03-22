using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    [Table("EmployeeBusinessTrip")]
    public class EmployeeBusinessTrip : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public long UserId { get; set; }


        public string Destination { get; set; }

        public DateTime PreBeginDate { get; set; }

        public DateTime PreEndDate { get; set; }

        public string PreSchedule { get; set; }


        public DateTime? BeginDate { get; set; }


        public DateTime? EndDate { get; set; }


        public string Schedule { get; set; }


        public string FeePlan { get; set; }

        public decimal PreFeeTotal { get; set; }

        public decimal FeeTotal { get; set; }

        public decimal FeeAccommodation { get; set; }

        public decimal FeeOther { get; set; }


        public int Status { get; set; }


        public string Remark { get; set; }




    }
}
