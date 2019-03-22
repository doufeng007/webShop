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
    /// oa车辆管理
    /// </summary>
    [Table("OACar")]
    public  class OACar : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Banrd { get; set; }
        /// <summary>
        /// 购买时间
        /// </summary>
        public DateTime? BuyTime { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal? Price { get; set; }
    }
}
