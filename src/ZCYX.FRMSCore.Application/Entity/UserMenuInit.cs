using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    [Table("UserMenuInit")]
    public class UserMenuInit : FullAuditedEntity<Guid>
    {
        public long UserId { get; set; }


        public long MenuId { get; set; }


        public int Status { get; set; }


    }
}
