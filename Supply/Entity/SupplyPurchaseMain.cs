using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Supply
{

    [Table("SupplyPurchaseMain")]
    public class SupplyPurchaseMain : FullAuditedEntity<Guid>
    {

        public string Code { get; set; }

        public int Status { get; set; }

    }
}
