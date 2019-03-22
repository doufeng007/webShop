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
using ZCYX.FRMSCore;

namespace Project
{
    [Serializable]
    [TableNameAtribute("CJZ比对结果")]
    [Table("CJZFileCompareResult")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class CJZFileCompareResult : FullAuditedEntity<Guid>
    {

        public CJZFileCompareResult DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as CJZFileCompareResult;
            }
        }

        #region 表字段

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Pro_Id")]
        public Guid Pro_Id { get; set; }


        [DisplayName("SourceFileId")]
        public string SourceFileId { get; set; }


        [DisplayName("TargetFileId")]
        public string TargetFileId { get; set; }


        public long? SourceUserId { get; set; }


        public long? TargetUserId { get; set; }


        [DisplayName("CompareType")]
        public int CompareType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Remark")]
        public string Remark { get; set; }


        public string Result { get; set; }

        #endregion

    }


}
