using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    /// <summary>
    /// 项目工程评审-评审人提出疑问等待回复
    /// </summary>
    public class ProjectQuestion : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 回复
        /// </summary>
        public string Answer { get; set; }
    }
}
