using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    [Serializable]
    public class DocmentBorrow : FullAuditedEntity<Guid>
    {
        //[Obsolete]
        //public Guid DocmentId { get; set; }
        
        public string Des { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? BackTime { get; set; }
        public int? Count { get; set; }
        /// <summary>
        /// 借阅验证码
        /// </summary>
        public string Verify { get; set; }
        public int Status { get; set; }
        public string DealWithUsers { get; set; }
        /// <summary>
        /// 外部借阅人
        /// </summary>
        public string OutUser { get; set; }
        /// <summary>
        /// 外部借阅人电话
        /// </summary>
        public string OutPhone { get; set; }
        /// <summary>
        /// 外部借阅人单位
        /// </summary>
        public string OutCompany { get; set; }
        /// <summary>
        /// 是否外部借阅申请
        /// </summary>
        public bool? IsOut { get; set; }
    }
}
