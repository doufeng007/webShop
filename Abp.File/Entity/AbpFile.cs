using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.File
{
    [Table("AbpFile")]
    public class AbpFile : FullAuditedEntity<Guid>, IMayHaveTenant
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

        [DisplayName("TurnType")]
        public TurnType TurnType { get; set; }

        [DisplayName("TurnFileId")]
        public Guid? TurnFileId { get; set; }

        public int? TenantId { get; set; }
    }
}