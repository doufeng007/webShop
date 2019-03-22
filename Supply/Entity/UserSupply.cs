using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Supply
{

    [Table("UserSupply")]
    public class UserSupply : FullAuditedEntity<Guid>
    {
        public long UserId { get; set; }

        public Guid SupplyId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int Status { get; set; }


    }
}
