using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Project
{
    /// <summary>
    /// 项目进度完成情况
    /// </summary>
    [Table("ProjectProgressComplate")]
    public class ProjectProgressComplate : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectBaseId { get; set; }
        /// <summary>
        /// 初审完成时间
        /// </summary>
        public DateTime? FirstAuditComplateTime { get; set; }
        /// <summary>
        /// 初审延后小时
        /// </summary>
        public int? FirstAduitDelayHour { get; set; }
        /// <summary>
        /// 计量完成时间
        /// </summary>
        public DateTime? JiliangComplateTime { get; set; }
        /// <summary>
        /// 计量延后小时
        /// </summary>
        public int? JiliangDelayHour { get; set; }
        /// <summary>
        /// 计价完成时间
        /// </summary>
        public DateTime? JijiaComplateTime { get; set; }
        /// <summary>
        /// 计价延后小时
        /// </summary>
        public int? JijiaDelayHour { get; set; }
        /// <summary>
        /// 内核完成时间
        /// </summary>
        public DateTime? SelfAuditComplateTime { get; set; }
        /// <summary>
        /// 内核延后小时
        /// </summary>
        public int? SelfAuditDelayHour { get; set; }
        /// <summary>
        /// 复核完成时间
        /// </summary>
        public DateTime? SecondAuditComplateTime { get; set; }
        /// <summary>
        /// 复核延后小时
        /// </summary>
        public int? SecondAuditDelayHour { get; set; }
        /// <summary>
        /// 总核完成时间
        /// </summary>
        public DateTime? LastAuditComplateTime { get; set; }
        /// <summary>
        /// 总核延后小时
        /// </summary>
        public int? LastAuditDelayHour { get; set; }



        public int Status { get; set; }
    }

}
