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
    [Table("PerformanceAppealDetail")]
    public class PerformanceAppealDetail : Entity<Guid>
    {
        #region 表字段
        
        /// <summary>
        /// 绩效编号
        /// </summary>
        [DisplayName(@"绩效编号")]
        public Guid PerformanceId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName(@"类型")]
        public PerformanceType Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName(@"内容")]
        public string Content { get; set; } 
        /// <summary>
        /// 申诉编号
        /// </summary>
        [DisplayName(@"申诉编号")]
        public Guid AppealId { get; set; }
        /// <summary>
        /// 申诉前得分
        /// </summary>
        [DisplayName(@"申诉前得分")]
        public int Score { get; set; }
        /// <summary>
        /// 申诉后得分
        /// </summary>
        [DisplayName(@"申诉后得分")]
        public int? AfterScore { get; set; }


        #endregion
    }
}