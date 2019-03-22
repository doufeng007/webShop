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
    /// OA绩效考评主表
    /// </summary>
    [Table("OAPerformanceMain")]
    public class OAPerformanceMain : FullAuditedEntity<Guid>
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
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 参与员工
        /// </summary>
        public string AuditUser { get; set; }
        public string Des { get; set; }
    }
}
