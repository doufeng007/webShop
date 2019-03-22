using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Project
{
    [Table("ProjectFileAdditional")]
    public class ProjectFileAdditional : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// FileTypeName
        /// </summary>
        [DisplayName(@"FileTypeName")]
        [MaxLength(200)]
        public string FileTypeName { get; set; }

        /// <summary>
        /// ProjectBaseId
        /// </summary>
        [DisplayName(@"ProjectBaseId")]
        public Guid ProjectBaseId { get; set; }

        /// <summary>
        /// PaperFileNumber
        /// </summary>
        [DisplayName(@"PaperFileNumber")]
        public int? PaperFileNumber { get; set; }

        /// <summary>
        /// IsPaperFile
        /// </summary>
        [DisplayName(@"IsPaperFile")]
        public bool IsPaperFile { get; set; }

        /// <summary>
        /// IsNeedReturn
        /// </summary>
        [DisplayName(@"IsNeedReturn")]
        public bool IsNeedReturn { get; set; }


        #endregion
    }
}