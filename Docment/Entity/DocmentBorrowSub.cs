using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    public class DocmentBorrowSub: AuditedEntity<Guid>
    {
        /// <summary>
        /// 借阅申请id
        /// </summary>
        public Guid BorrowId { get; set; }
        /// <summary>
        /// 档案id
        /// </summary>
        public Guid DocmentId { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime? GetTime { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? BackTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public BorrowSubStatus Status { get; set; }
    }

    public enum BorrowSubStatus {
        审核中=0,
        同意=1,
        待领取=2,
        驳回=3,
        使用中=4,
        已归还=5
    }
}
