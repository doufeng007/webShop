using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    [Table("PostInfo")]
    public class PostInfo : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }


        public int Status { get; set; }


    }
}
