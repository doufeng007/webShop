using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply.Entity
{
    public class SupplyApplySub : FullAuditedEntity<Guid>
    {

        public int Status { get; set; }

        public Guid MainId { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public string Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }

        public Guid? SupplyId { get; set; }

        public string UserId { get; set; }

        public int Type { get; set; }

        public int Result { get; set; }

        public string ResultRemark { get; set; }

        
    }

}
