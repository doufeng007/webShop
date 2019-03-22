using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
namespace Project
{


    [Serializable]
    [Table("ProjectFile")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectFile : FullAuditedEntity<Guid>
    {


        public ProjectFile DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as ProjectFile;
            }
        }

        #region 表字段
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectBaseId")]
        public Guid ProjectBaseId { get; set; }


        public Guid SingleProjectId { get; set; }

        
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("AappraisalFileType")]
        public int AappraisalFileType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasUpload")]
        public bool HasUpload { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FileName")]
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FilePath")]
        public string FilePath { get; set; }

        [DisplayName("IsPaperFile")]
        public bool IsPaperFile { get; set; }


        public bool IsMust { get; set; }

        public bool? Back { get; set; }


        public int? PaperFileNumber { get; set; }

        #endregion

    }
}
