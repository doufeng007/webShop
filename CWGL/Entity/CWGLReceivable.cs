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
    [Table("CWGLReceivable")]
    public class CWGLReceivable : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 收款人
        /// </summary>
        [DisplayName(@"收款人")]
        [MaxLength(20)]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [DisplayName(@"客户名称")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DisplayName(@"金额")]
        public decimal Money { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        [DisplayName(@"收款方式")]
        public MoneyMode Mode { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [DisplayName(@"银行名称")]
        [MaxLength(100)]
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
        [MaxLength(100)]
        public string BankOpenName { get; set; }

        /// <summary>
        /// 事由摘要
        /// </summary>
        [DisplayName(@"事由摘要")]
        [Required]
        public string Note { get; set; }

        /// <summary>
        /// 纸质凭证
        /// </summary>
        [DisplayName(@"纸质凭证")]
        public int? Nummber { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [DisplayName(@"流程查阅人员")]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }


        #endregion
    }
}