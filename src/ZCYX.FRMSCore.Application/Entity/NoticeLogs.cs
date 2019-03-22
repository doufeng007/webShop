using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZCYX.FRMSCore.Application
{
    [Table("NoticeLogs")]
    public class NoticeLogs : FullAuditedEntity<Guid>
    {
      
        /// <summary>
        /// 公告ID
        /// </summary>
        [DisplayName("TextId")]
        public Guid TextId { get; set; }

        /// <summary>
        /// 接收者ID
        /// </summary>
        [DisplayName("ReceiveId")]
        public long ReceiveId { get; set; }

        /// <summary>
        /// 状态
        /// 1未读  2已读 3已删
        /// </summary>
        [DisplayName("Status")]
        public Int32 Status { get; set; }

        /// <summary>
        /// 查看时间
        /// </summary>
        [DisplayName("ReadTime")]
        public DateTime? ReadTime { get; set; }

        /// <summary>
        /// 类型
        /// 1业务通知  2公告
        /// </summary>
        [DisplayName("NoticeType")]
        public int NoticeType { get; set; }
    }
}