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
    /// <summary>
    /// 提货
    /// </summary>
    [Serializable]
    [Table("B_OrderOut")]
    public class B_OrderOut : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        [DisplayName(@"Amout")]
        public decimal Amout { get; set; }

        /// <summary>
        /// DeliveryFee
        /// </summary>
        [DisplayName(@"DeliveryFee")]
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// PayAmout
        /// </summary>
        [DisplayName(@"PayAmout")]
        public decimal PayAmout { get; set; }

        /// <summary>
        /// GoodsPayment
        /// </summary>
        [DisplayName(@"GoodsPayment")]
        public decimal GoodsPayment { get; set; }

        /// <summary>
        /// Balance
        /// </summary>
        [DisplayName(@"Balance")]
        public decimal Balance { get; set; }

        /// <summary>
        /// AddressId
        /// </summary>
        [DisplayName(@"AddressId")]
        public Guid AddressId { get; set; }

        /// <summary>
        /// Stauts
        /// </summary>
        [DisplayName(@"Stauts")]
        public OrderOutStauts Stauts { get; set; }

        public string Remark { get; set; }


        #endregion
    }
}