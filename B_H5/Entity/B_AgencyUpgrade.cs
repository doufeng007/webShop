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
    [Table("B_AgencyUpgrade")]
    public class B_AgencyUpgrade : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// AgencyId
        /// </summary>
        [DisplayName(@"AgencyId")]
        public Guid AgencyId { get; set; }

        /// <summary>
        /// ToAgencyLevelId
        /// </summary>
        [DisplayName(@"ToAgencyLevelId")]
        public Guid ToAgencyLevelId { get; set; }

        /// <summary>
        /// NeedPrePayAmout
        /// </summary>
        [DisplayName(@"NeedPrePayAmout")]
        public decimal NeedPrePayAmout { get; set; }

        /// <summary>
        /// NeedDeposit
        /// </summary>
        [DisplayName(@"NeedDeposit")]
        public decimal NeedDeposit { get; set; }


        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public B_AgencyApplyStatusEnum Status { get; set; }


        /// <summary>
        /// PayType
        /// </summary>
        [DisplayName(@"PayType")]
        public PayAccountType PayType { get; set; }

        /// <summary>
        /// PayAmout
        /// </summary>
        [DisplayName(@"PayAmout")]
        public decimal PayAmout { get; set; }

        /// <summary>
        /// PayAcount
        /// </summary>
        [DisplayName(@"PayAcount")]
        [MaxLength(50)]
        [Required]
        public string PayAcount { get; set; }


        /// <summary>
        /// 银行户名
        /// </summary>
        [MaxLength(50)]
        public string BankUserName { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        [MaxLength(50)]
        public string BankName { get; set; }

        /// <summary>
        /// PayDate
        /// </summary>
        [DisplayName(@"PayDate")]
        public DateTime PayDate { get; set; }


        public string Reason { get; set; }


        public string Remark { get; set; }

        #endregion
    }
}