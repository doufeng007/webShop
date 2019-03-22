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
    [Serializable]
    [Table("CWGLPrePayment")]
    public class CWGLPrePayment : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
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
        /// 应收金额
        /// </summary>
        [DisplayName(@"应收金额")]
        public decimal Money { get; set; }

        /// <summary>
        /// 结清状态
        /// </summary>
        [DisplayName(@"结清状态")]
        public int SettleState { get; set; }


        #endregion
    }
}