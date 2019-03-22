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
    [Table("Performance")]
    public class Performance : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 用户
        /// </summary>
        [DisplayName(@"用户")]
        public long UserId { get; set; }

        /// <summary>
        /// 事项
        /// </summary>
        [DisplayName(@"事项")]
        public string Matter { get; set; }

        /// <summary>
        /// 记录
        /// </summary>
        [DisplayName(@"记录")]
        [Required]
        public string Record { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        [DisplayName(@"分值")]
        public int Score { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName(@"类型")]
        public PerformanceType Type { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName(@"类型")]
        public int ScoreTypeId { get; set; }


        #endregion
    }
}