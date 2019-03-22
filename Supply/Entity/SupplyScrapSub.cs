using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Supply
{

    [Table("SupplyScrapSub")]
    public class SupplyScrapSub : FullAuditedEntity<Guid>
    {
        public Guid? MainId { get; set; }

        public Guid SupplyId { get; set; }


        public Guid UserSupplyId { get; set; }

        public int Status { get; set; }


        public decimal? PreResidueValue { get; set; }


        public string Reason { get; set; }


    }
}
