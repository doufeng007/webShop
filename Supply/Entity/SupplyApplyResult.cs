using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply
{
    
    public class SupplyApplyResult : FullAuditedEntity<Guid>
    {
        public int Status { get; set; }

        public Guid ApplyMainId { get; set; }

        public Guid ApplySubId { get; set; }

        public Guid SupplyId { get; set; }
       
    }

  
}
