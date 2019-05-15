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
    [Table("B_OrderOutBonus")]
    public class B_OrderOutBonus : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// Amout
        /// </summary>
        [DisplayName(@"Amout")]
        public decimal Amout { get; set; }

        /// <summary>
        /// EffectTime
        /// </summary>
        [DisplayName(@"EffectTime")]
        public DateTime EffectTime { get; set; }

        /// <summary>
        /// FailureTime
        /// </summary>
        [DisplayName(@"FailureTime")]
        public DateTime? FailureTime { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public BonusRuleStatusEnum Status { get; set; }


        #endregion
    }
}