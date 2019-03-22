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
    [Table("ArchivesManager")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ArchivesManager : FullAuditedEntity<Guid>
    {
        #region 表字段

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ArchivesType")]
        public int ArchivesType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ArchivesNumber")]
        public string ArchivesNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("VolumeNumber")]
        public string VolumeNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ArchivesName")]
        public string ArchivesName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectId")]
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Location")]
        public string Location { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SecrecyLevel")]
        public int? SecrecyLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ArchivesNumber1")]
        public string ArchivesNumber1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("PageNumber")]
        public int? PageNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Summary")]
        public string Summary { get; set; }


        public int Status { get; set; }



        #endregion
    }


  


}
