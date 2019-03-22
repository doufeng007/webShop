using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.File
{
    [Table("AbpFileRelation")]
    public class AbpFileRelation : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        [DisplayName("BusinessType")]
        public int BusinessType { get; set; }

        [DisplayName("BusinessId")]
        public string BusinessId { get; set; }

        [DisplayName("FileId")]
        public Guid FileId { get; set; }


        [DisplayName("Sort")]
        public int Sort { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        public int? TenantId { get; set; }
    }
}