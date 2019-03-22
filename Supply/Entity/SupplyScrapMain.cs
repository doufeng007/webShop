using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Supply
{

    [Table("SupplyScrapMain")]
    public class SupplyScrapMain : FullAuditedEntity<Guid>
    {


        public string Remark { get; set; }

        public int Status { get; set; }


        //public string Reason { get; set; }


    }
}
