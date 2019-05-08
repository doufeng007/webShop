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
    [Table("B_Order")]
    public class B_Order : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段


        public string OrderNo { get; set; }

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
        /// Stauts
        /// </summary>
        [DisplayName(@"Stauts")]
        public int Stauts { get; set; }

        public OrderAmoutEnum InOrOut { get; set; }

        public Guid BusinessId { get; set; }

        public OrderAmoutBusinessTypeEnum BusinessType { get; set; }


        /// <summary>
        /// 是余额
        /// </summary>
        public bool IsBlance { get; set; }

        /// <summary>
        /// 是货款
        /// </summary>
        public bool IsGoodsPayment { get; set; }


        #endregion
    }
}