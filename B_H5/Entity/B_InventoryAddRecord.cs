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
    [Table("B_InventoryAddRecord")]
    public class B_InventoryAddRecord : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// Goodsid
        /// </summary>
        [DisplayName(@"Goodsid")]
        public Guid Goodsid { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        [DisplayName(@"Count")]
        public int Count { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public B_InventoryAddConfigEnum Status { get; set; }

        /// <summary>
        /// ConfirmUserId
        /// </summary>
        [DisplayName(@"ConfirmUserId")]
        public long? ConfirmUserId { get; set; }


        #endregion
    }
}