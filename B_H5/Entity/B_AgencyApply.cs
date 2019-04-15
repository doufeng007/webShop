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
    [Table("B_AgencyApply")]
    public class B_AgencyApply : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段


        public Guid InviteUrlId { get; set; }


        public string Name { get; set; }
        /// <summary>
        /// AgencyLevelId
        /// </summary>
        [DisplayName(@"AgencyLevelId")]
        public Guid AgencyLevelId { get; set; }

        /// <summary>
        /// AgencyLevel
        /// </summary>
        [DisplayName(@"AgencyLevel")]
        public int AgencyLevel { get; set; }

        /// <summary>
        /// Tel
        /// </summary>
        [DisplayName(@"Tel")]
        [MaxLength(50)]
        [Required]
        public string Tel { get; set; }

        /// <summary>
        /// VCode
        /// </summary>
        [DisplayName(@"VCode")]
        [MaxLength(50)]
        [Required]
        public string VCode { get; set; }

        /// <summary>
        /// Pwd
        /// </summary>
        [DisplayName(@"Pwd")]
        [MaxLength(50)]
        [Required]
        public string Pwd { get; set; }

        /// <summary>
        /// WxId
        /// </summary>
        [DisplayName(@"WxId")]
        [MaxLength(100)]
        public string WxId { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        [DisplayName(@"Country")]
        [MaxLength(100)]
        public string Country { get; set; }

        /// <summary>
        /// PNumber
        /// </summary>
        [DisplayName(@"PNumber")]
        [MaxLength(50)]
        [Required]
        public string PNumber { get; set; }

        /// <summary>
        /// Provinces
        /// </summary>
        [DisplayName(@"Provinces")]
        [MaxLength(100)]
        public string Provinces { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [DisplayName(@"City")]
        [MaxLength(100)]
        public string City { get; set; }

        /// <summary>
        /// County
        /// </summary>
        [DisplayName(@"County")]
        [MaxLength(100)]
        public string County { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        [DisplayName(@"Address")]
        [MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// PayType
        /// </summary>
        [DisplayName(@"PayType")]
        public int PayType { get; set; }

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

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public B_AgencyApplyStatusEnum Status { get; set; }


        [MaxLength(200)]
        public string Remark { get; set; }


        public string Reason { get; set; }

        


        #endregion
    }
}