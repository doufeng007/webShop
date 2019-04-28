using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;

namespace B_H5
{
    [Serializable]
    [Table("B_OrderIn")]
    public class B_OrderIn : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// Amout
        /// </summary>
        [DisplayName(@"Amout")]
        public decimal Amout { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public InOrderStatusEnum Status { get; set; }

        public Guid CategroyId { get; set; }

        /// <summary>
        /// 进货数量
        /// </summary>
        public int Number { get; set; }


        /// <summary>
        /// 货款
        /// </summary>
        public decimal GoodsPayment { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }


        #endregion
    }
}