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
using TaskGL.Enum;

namespace TaskGL
{
    [Serializable]
    [Table("TaskManagementChange")]
    public class TaskManagementChange : Entity<Guid>
    {
        #region 表字段
        
        /// <summary>
        /// 变更类型
        /// </summary>
        [DisplayName(@"变更类型")]
        public TaskManagementChangeTypeEnum Type { get; set; }

        /// <summary>
        /// 变更编号
        /// </summary>
        [DisplayName(@"变更编号")]
        public Guid TaskManagementId { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        [DisplayName(@"原因")]
        [Required]
        public TaskManagementReasonEnum Reason { get; set; }

        /// <summary>
        /// 评估
        /// </summary>
        [DisplayName(@"评估")]
        public string Assessment { get; set; }

        /// <summary>
        /// 要求
        /// </summary>
        [DisplayName(@"要求")]
        public string Requirement { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [DisplayName(@"创建者")]
        public long CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName(@"创建时间")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 绩效分
        /// </summary>
        [DisplayName(@"绩效分")]
        public int? PerformanceScore { get; set; }

        /// <summary>
        /// 精神分
        /// </summary>
        [DisplayName(@"精神分")]
        public int? SpiritScore { get; set; }

        #endregion
    }
}