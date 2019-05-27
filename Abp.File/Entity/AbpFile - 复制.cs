using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.File
{
    [Table("AbpFileThumb")]
    public class AbpFileThumb : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        [DisplayName("FileName")]
        public string FileName { get; set; }

        [DisplayName("FileSize")]
        public long FileSize { get; set; }

        [DisplayName("FilePath")]
        public string FilePath { get; set; }

        [DisplayName("FileExtend")]
        public string FileExtend { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }


        public Guid OrgFileId { get; set; }

      
        public int? TenantId { get; set; }
    }
}