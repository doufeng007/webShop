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
    [Table("QuestionLibrary")]
    public class QuestionLibrary : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName(@"标题")]
        [MaxLength(10)]
        public string Title { get; set; }


        [DisplayName(@"题库分类")]
        public Guid TypeId { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName(@"描述")]
        [MaxLength(50)]
        public string Remark { get; set; }


        #endregion
    }
}