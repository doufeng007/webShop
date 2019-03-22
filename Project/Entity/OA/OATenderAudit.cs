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
    /// 投标文件审查
    /// </summary>
    [Table("OATenderAudit")]
    public class OATenderAudit : FullAuditedEntity<Guid>
    {
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// 投标金额
        /// </summary>
        public decimal? TenderPrice { get; set; }
        /// <summary>
        /// 建设单位
        /// </summary>
        public string Builder { get; set; }

        public string Content { get; set; }

        public string Files { get; set; }

        public int Status { get; set; }

        public string AuditUser { get; set; }
    }
}
