using Abp.Domain.Entities.Auditing;
using Abp.WeChat.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Abp.WeChat.Entity
{

    [Table("AbpWxMessage")]
    public class AbpWxMessage : FullAuditedEntity<Guid>
    {

        public string BusinessId { get; set; }


        public TemplateMessageBusinessTypeEnum BType { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string LinkUrl { get; set; }


        public string ReceiveOpenId { get; set; }

        public int SendStatus { get; set; }


        public string SendError { get; set; }


        public DateTime? ReceiveTime { get; set; }

        public string MsgID { get; set; }

        public int Status { get; set; }

        public int Sort { get; set; }

    }
}
