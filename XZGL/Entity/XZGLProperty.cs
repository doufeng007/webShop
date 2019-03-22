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

namespace XZGL
{
    [Serializable]
    [Table("XZGLProperty")]
    public class XZGLProperty : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [DisplayName(@"Number")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName(@"类型")]
        public Guid Type { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName(@"类型")]
        public string TypeName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName(@"名称")]
        [MaxLength(300)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [DisplayName(@"负责人")]
        public long? UserId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [DisplayName(@"是否启用")]
        public bool? IsEnable { get; set; }


        #endregion
    }
}