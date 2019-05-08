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
    [Table("B_AgencySalesDetail")]
    public class B_AgencySalesDetail : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// Pid
        /// </summary>
        [DisplayName(@"Pid")]
        public Guid Pid { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        [DisplayName(@"CategroyId")]
        public Guid CategroyId { get; set; }

        /// <summary>
        /// Sales
        /// </summary>
        [DisplayName(@"Sales")]
        public decimal Sales { get; set; }

        /// <summary>
        /// Profit
        /// </summary>
        [DisplayName(@"Profit")]
        public decimal Profit { get; set; }

        /// <summary>
        /// Discount
        /// </summary>
        [DisplayName(@"Discount")]
        public decimal Discount { get; set; }


        #endregion
    }
}