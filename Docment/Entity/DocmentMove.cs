using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    /// <summary>
    /// 档案移交申请
    /// </summary>
    [Serializable]
    public class DocmentMove : FullAuditedEntity<Guid>
    {
        public Guid DocmentId { get; set; }
        /// <summary>
        /// 档案编号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 存放位置
        /// </summary>
        public string Location { get; set; }
        public string Des { get; set; }

        public int Status { get; set; }
        public string DealWithUsers { get; set; }
    }
}
