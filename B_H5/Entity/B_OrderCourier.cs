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
    [Table("B_OrderCourier")]
    public class B_OrderCourier : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// OrderId
        /// </summary>
        [DisplayName(@"OrderId")]
        public Guid OrderId { get; set; }

        /// <summary>
        /// CourierNum
        /// </summary>
        [DisplayName(@"CourierNum")]
        [MaxLength(100)]
        [Required]
        public string CourierNum { get; set; }

        /// <summary>
        /// CourierName
        /// </summary>
        [DisplayName(@"CourierName")]
        [MaxLength(50)]
        [Required]
        public string CourierName { get; set; }

        /// <summary>
        /// DeliveryFee
        /// </summary>
        [DisplayName(@"DeliveryFee")]
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }


        #endregion
    }
}