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
    [Table("PerformanceScore")]
    public class PerformanceScore : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 规则
        /// </summary>
        [DisplayName(@"规则")]
        [MaxLength(300)]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 小于等于
        /// </summary>
        [DisplayName(@"小于等于")]
        public int Than1 { get; set; }

        /// <summary>
        /// 区间大于
        /// </summary>
        [DisplayName(@"区间大于")]
        public int Than21 { get; set; }

        /// <summary>
        /// 区间小于等于
        /// </summary>
        [DisplayName(@"区间小于等于")]
        public int Than22 { get; set; }

        /// <summary>
        /// 大于
        /// </summary>
        [DisplayName(@"大于")]
        public int Than3 { get; set; }

        /// <summary>
        /// 小于等于分数
        /// </summary>
        [DisplayName(@"小于等于分数")]
        public int Than1Score { get; set; }

        /// <summary>
        /// 区间分数
        /// </summary>
        [DisplayName(@"区间分数")]
        public int Than2Score { get; set; }

        /// <summary>
        /// 大于分数
        /// </summary>
        [DisplayName(@"大于分数")]
        public int Than3Score { get; set; }

        /// <summary>
        /// 分数类型
        /// </summary>
        [DisplayName(@"分数类型")]
        public int ScoreTypeId { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [DisplayName(@"单位")]
        public PerformanceUnit Unit { get; set; }

        /// <summary>
        /// 加分减分选择
        /// </summary>
        [DisplayName(@"加分减分选择")]
        public PerformanceScoreTypeEnum ScoreType { get; set; }


        #endregion
    }
}