using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Project
{

    [Serializable]
    [Table("CJZFileCompareResultStatistics")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class CJZFileCompareResultStatistics : FullAuditedEntity<Guid>
    {
        #region 表字段


        /// <summary>
        /// 
        /// </summary>
        public Guid Pro_Id { get; set; }


        public Guid CompareId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }

        public Guid CheckRoleId { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal SinglePrice { get; set; }


        public decimal Fee { get; set; }

      



        #endregion

    }
}
