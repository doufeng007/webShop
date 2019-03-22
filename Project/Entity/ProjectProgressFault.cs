using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    /// <summary>
    /// 项目进度故障
    /// </summary>
    public class ProjectProgressFault : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 项目进度
        /// </summary>
        public Guid ProgressComplate { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        public ProjectStatus FaultType { get; set; }

        public string Content { get; set; }
    }
}
