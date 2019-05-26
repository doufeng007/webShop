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
    [Table("B_AgencyLevel")]
    public class B_AgencyLevel : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// Name
        /// </summary>
        [DisplayName(@"Name")]
        [MaxLength(500)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        [DisplayName(@"Level")]
        public int Level { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        [DisplayName(@"IsDefault")]
        public bool IsDefault { get; set; }


        /// <summary>
        /// 首充金额
        /// </summary>
        public decimal FirstRechargeAmout { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal Deposit { get; set; }


        /// <summary>
        /// 推荐奖
        /// </summary>
        public decimal RecommendAmout { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; } = 1;


        #endregion
    }
}