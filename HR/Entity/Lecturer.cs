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

namespace HR
{
    [Serializable]
    [Table("Lecturer")]
    public class Lecturer : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 讲师姓名
        /// </summary>
        [DisplayName(@"讲师姓名")]
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 课时费
        /// </summary>
        [DisplayName(@"课时费")]
        public decimal TeachSubsidy { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [DisplayName(@"电话")]
        [MaxLength(11)]
        [Required]
        public string Tel { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DisplayName(@"邮箱")]
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [DisplayName(@"银行卡号")]
        [MaxLength(24)]
        [Required]
        public string BankId { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        [DisplayName(@"银行")]
        [MaxLength(50)]
        [Required]
        public string Bank { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        [DisplayName(@"开户行")]
        [MaxLength(50)]
        [Required]
        public string OpenBank { get; set; }

        /// <summary>
        /// 讲师简介
        /// </summary>
        [DisplayName(@"讲师简介")]
        [MaxLength(500)]
        public string Introduction { get; set; }


        #endregion
    }
}