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
    [Table("CWGLoan")]
    public class CWGLoan : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 贷款金额
        /// </summary>
        [DisplayName(@"贷款金额")]
        public decimal Money { get; set; }

        /// <summary>
        /// 贷款用途及备注
        /// </summary>
        [DisplayName(@"贷款用途及备注")]
        [MaxLength(300)]
        public string Remark { get; set; }

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


        #endregion
    }
}