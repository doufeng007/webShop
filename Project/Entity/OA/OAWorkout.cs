using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// OA外出
    /// </summary>
    [Serializable]
    [Table("OAWorkout")]
    public class OAWorkout : FullAuditedEntity<Guid>
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
        /// 出差地点
        /// </summary>
        public string Destination { get; set; }




        public string DealWithUsers { get; set; }

        /// <summary>
        /// 出发地点
        /// </summary>
        public string FromPosition { get; set; }

        /// <summary>
        /// 交通工具
        /// </summary>
        public Guid TranType { get; set; }


        public long OrgId { get; set; }


        public string PostIds { get; set; }
        public long? RelationUserId { get; set; }
        public bool? IsCar { get; set; }
    }
}
