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
    [Table("EmployeeResume")]
    public class EmployeeResume : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 姓名
        /// </summary>
        [DisplayName(@"姓名")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DisplayName(@"性别")]
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DisplayName(@"邮箱")]
        [MaxLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [DisplayName(@"年龄")]
        public int Age { get; set; }

        /// <summary>
        /// 期望职位
        /// </summary>
        [DisplayName(@"期望职位")]
        [MaxLength(20)]
        public string Position { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DisplayName(@"手机")]
        [MaxLength(11)]
        public string Phone { get; set; }

        /// <summary>
        /// 居住地
        /// </summary>
        [DisplayName(@"居住地")]
        [MaxLength(100)]
        public string Address { get; set; }

        /// <summary>
        /// 薪酬
        /// </summary>
        [DisplayName(@"薪酬")]
        public decimal? Salary { get; set; }

        /// <summary>
        /// 薪酬(起薪)
        /// </summary>
        [DisplayName(@"薪酬(起薪)")]
        public decimal? StartingSalary { get; set; }

        /// <summary>
        /// 薪酬(是否面议)
        /// </summary>
        [DisplayName(@"薪酬(是否面议)")]
        public bool IsFace { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        [MaxLength(20)]
        public string Remark { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        /// <summary>
        /// 岗位经验
        /// </summary>
        [DisplayName(@"岗位经验")]
        public int Experience { get; set; }

        [DisplayName(@"状态")]
        public ResumeStatus Status { get; set; }
        #endregion
    }
    public enum ResumeStatus {
        新增,
        淘汰,
        离职,
        已入职,
        未入职,
        面试终止,
        面试中,
        未面试

    }
}