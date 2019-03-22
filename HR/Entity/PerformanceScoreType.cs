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
    [Table("PerformanceScoreType")]
    public class PerformanceScoreType : Entity<int>
    {
        #region 表字段
        
        /// <summary>
        /// 绩效标题
        /// </summary>
        [DisplayName(@"绩效标题")]
        [MaxLength(300)]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 是否编辑
        /// </summary>
        [DisplayName(@"是否编辑")]
        public bool? IsEdit { get; set; }

        /// <summary>
        /// 父级编号
        /// </summary>
        [DisplayName(@"父级编号")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName(@"排序")]
        public int? Sort { get; set; }


        #endregion
    }
}