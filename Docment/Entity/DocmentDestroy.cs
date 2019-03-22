using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    /// <summary>
    /// 档案销毁
    /// </summary>
    [Serializable]
    public class DocmentDestroy : FullAuditedEntity<Guid>
    {
        public Guid DocmentId { get; set; }

        public string Des { get; set; }

        public int Status { get; set; }
        public string DealWithUsers { get; set; }
    }
}
