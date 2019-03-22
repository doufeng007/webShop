using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// 投标业务费申请
    /// </summary>
    [Table("OATenderBuess")]
    public class OATenderBuess : FullAuditedEntity<Guid>
    {
        public string Code { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectType { get; set; }
        /// <summary>
        /// 申请金额
        /// </summary>
        public decimal? CashPrice { get; set; }

        public string CashPriceUp { get; set; }

        public string Des { get; set; }

        //public string File { get; set; }

        public int Status { get; set; }


        //public DateTime ApplyDate { get; set; }

        //public string ApplyUser { get; set; }
    }
}
