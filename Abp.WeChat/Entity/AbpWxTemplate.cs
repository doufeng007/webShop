using Abp.Domain.Entities.Auditing;
using Abp.WeChat.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Abp.WeChat.Entity
{

    [Table("AbpWxTemplate")]
    public class AbpWxTemplate : FullAuditedEntity<Guid>
    {
        public TemplateMessageBusinessTypeEnum BType { get; set; }

        public string WxTemplateId { get; set; }


    }
}
