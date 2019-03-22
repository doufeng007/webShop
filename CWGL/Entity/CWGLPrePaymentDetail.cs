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
    [Table("CWGLPrePaymentDetail")]
    public class CWGLPrePaymentDetail : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 预收款编号
        /// </summary>
        [DisplayName(@"预收款编号")]
        public Guid PrePaymentId { get; set; }

        /// <summary>
        /// 预收金额
        /// </summary>
        [DisplayName(@"预收金额")]
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        [DisplayName(@"收款方式")]
        public int Mode { get; set; }

        /// <summary>
        /// 收款银行
        /// </summary>
        [DisplayName(@"收款银行")]
        [MaxLength(20)]
        public string BankName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        [DisplayName(@"卡号")]
        [MaxLength(64)]
        public string CardNumber { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        [DisplayName(@"开户行名称")]
        [MaxLength(32)]
        public string BankOpenName { get; set; }


        #endregion
    }
}