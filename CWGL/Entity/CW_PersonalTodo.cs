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
using CWGL.Enums;

namespace CWGL
{
    [Serializable]
    [Table("CW_PersonalTodo")]
    public class CW_PersonalTodo : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// BusinessId
        /// </summary>
        [DisplayName(@"BusinessId")]
        [MaxLength(100)]
        [Required]
        public string BusinessId { get; set; }

        /// <summary>
        /// BusinessType
        /// </summary>
        [DisplayName(@"BusinessType")]
        public CW_PersonalType BusinessType { get; set; }

        /// <summary>
        /// CWType
        /// </summary>
        [DisplayName(@"CWType")]
        public RefundResultType CWType { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        [DisplayName(@"Amout")]
        public decimal Amout_Pay { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Amout_Gather { get; set; }


        public bool HasSubmitPay { get; set; }

        public bool HasSubmitGather { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public CW_PersonalToStatus Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DisplayName(@"Remark")]
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// FlowId
        /// </summary>
        [DisplayName(@"FlowId")]
        public Guid? FlowId { get; set; }

        public string Title { get; set; }


        public long UserId { get; set; }
        #endregion
    }
}