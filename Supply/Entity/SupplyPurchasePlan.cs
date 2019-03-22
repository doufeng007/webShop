using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Supply
{

    [Table("SupplyPurchasePlan")]
    public class SupplyPurchasePlan : FullAuditedEntity<Guid>
    {
        public Guid? SupplyApplyMainId { get; set; }

        public Guid? SupplyApplySubId { get; set; }


        public Guid? SupplyPurchaseId { get; set; }

        public string SupplyPurchaseCode { get; set; }

        public int Status { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public string Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }

        public string UserId { get; set; }


        public int Type { get; set; }


        public int DoPurchaseStatus { get; set; }


        public DateTime? PutInDate { get; set; }


    }
}
