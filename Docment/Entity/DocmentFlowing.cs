using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    /// <summary>
    /// 档案流动记录
    /// </summary>
    public class DocmentFlowing: AuditedEntity<Guid>
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Des { get; set; }
        /// <summary>
        /// 是否外部流转
        /// </summary>
        public bool IsOut { get; set; }
        /// <summary>
        /// 档案
        /// </summary>
        public Guid DocmentId { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public string UserName { get; set; }
    }
}
