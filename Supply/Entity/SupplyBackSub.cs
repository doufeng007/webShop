using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply.Entity
{
    public class SupplyBackSub : FullAuditedEntity<Guid>
    {
        public int Status { get; set; }

        public Guid MainId { get; set; }

        public Guid SupplyId { get; set; }
    }
}
