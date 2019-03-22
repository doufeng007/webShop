using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Supply
{

    [Table("SupplyPurchaseResult")]
    public class SupplyPurchaseResult : FullAuditedEntity<Guid>
    {
        public Guid SupplyPurchasePlanId { get; set; }

        public Guid SupplyId { get; set; }

        public int Status { get; set; }

    }
}
