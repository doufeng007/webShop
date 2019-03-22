using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [Table("ArchivesFile")]
    public class ArchivesFile : FullAuditedEntity<Guid>
    {
        #region 表字段
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ArchivesId")]
        public Guid ArchivesId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FileName")]
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FileType")]
        public string FileType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("IsPaper")]
        public bool IsPaper { get; set; }

     

        #endregion

    }
}
