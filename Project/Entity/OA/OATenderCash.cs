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
    /// 投标保证金申请
    /// </summary>
    [Table("OATenderCash")]
    public class OATenderCash : FullAuditedEntity<Guid>
    {
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }
        /// <summary>
        /// 收款单位
        /// </summary>
        public string ToCompany { get; set; }

        public string BankName { get; set; }

        public string Account { get; set; }


        /// <summary>
        /// 保证金金额
        /// </summary>
        public decimal? CashPrice { get; set; }
        /// <summary>
        /// 金额大写
        /// </summary>
        public string CashPriceUp { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Des { get; set; }

        public string Files { get; set; }

        public int Status { get; set; }

        public string AuditUser { get; set; }
    }
}
