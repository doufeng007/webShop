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
    [Table("B_AgencySales")]
    public class B_AgencySales : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// AgencyId
        /// </summary>
        [DisplayName(@"AgencyId")]
        public Guid AgencyId { get; set; }


        public long FromUserId { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        [DisplayName(@"CategroyId")]
        public Guid CategroyId { get; set; }

        /// <summary>
        /// 销售额
        /// </summary>
        [DisplayName(@"Sales")]
        public decimal Sales { get; set; }

        /// <summary>
        /// SalesDate
        /// </summary>
        [DisplayName(@"SalesDate")]
        public DateTime SalesDate { get; set; }

        /// <summary>
        /// 利润
        /// </summary>
        [DisplayName(@"Profit")]
        public decimal Profit { get; set; }

        /// <summary>
        /// 销售折扣
        /// </summary>
        [DisplayName(@"Discount")]
        public decimal Discount { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Bonus { get; set; }


        #endregion
    }
}