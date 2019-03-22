using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    /// <summary>
    /// OA加班
    /// </summary>
    [Serializable]
    [Table("OAWorkon")]
    public class OAWorkon : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public decimal? Hours { get; set; }
        /// <summary>
        /// 外出原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string DealWithUsers { get; set; }
        public long OrgId { get; set; }

        public string PostIds { get; set; }
    }
}
