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

namespace HR
{
    [Table("WorkRecord")]
    [Serializable]
    public class WorkRecord : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// BusinessId
        /// </summary>
        [DisplayName(@"BusinessId")]
        public Guid BusinessId { get; set; }

        /// <summary>
        /// BusinessType
        /// </summary>
        [DisplayName(@"BusinessType")]
        public int BusinessType { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [DisplayName(@"Content")]
        [MaxLength(200)]
        public string Content { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        [DisplayName(@"StartTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [DisplayName(@"EndTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Remuneration
        /// </summary>
        [DisplayName(@"Remuneration")]
        public decimal Remuneration { get; set; }

        /// <summary>
        /// 数字化绩效
        /// </summary>
        [DisplayName(@"数字化绩效")]
        public decimal? DataPerformance { get; set; }    
        /// <summary>
        /// 非数字化绩效
        /// </summary>
        [DisplayName(@"非数字化绩效")]
        public decimal? NoDataPerformance { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }


        public string DealWithUsers { get; set; }


        

        #endregion
    }
}