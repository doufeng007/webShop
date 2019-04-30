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
    [Table("B_OrderDetail")]
    public class B_OrderDetail : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// BId
        /// </summary>
        [DisplayName(@"BId")]
        public Guid BId { get; set; }

        /// <summary>
        /// BType
        /// </summary>
        [DisplayName(@"BType")]
        public int BType { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [DisplayName(@"Number")]
        public int Number { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        [DisplayName(@"CategroyId")]
        public Guid CategroyId { get; set; }

        /// <summary>
        /// GoodsId
        /// </summary>
        [DisplayName(@"GoodsId")]
        public Guid GoodsId { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        [DisplayName(@"Amout")]
        public decimal Amout { get; set; }


        public long UserId { get; set; }


        #endregion
    }
}