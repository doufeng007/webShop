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

namespace CWGL
{
    [System.Serializable]
    [Table("CWGLAdvanceCharge")]
    public class CWGLAdvanceCharge : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [DisplayName(@"流程查阅人员")]
        [MaxLength(500)]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [DisplayName(@"客户名称")]
        [MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 事由说明
        /// </summary>
        [DisplayName(@"事由说明")]
        [MaxLength(300)]
        public string Cause { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>
        [DisplayName(@"应付金额")]
        public decimal Money { get; set; }

        /// <summary>
        /// 结清状态
        /// </summary>
        [DisplayName(@"结清状态")]
        public int SettleState { get; set; }


        #endregion
    }
}