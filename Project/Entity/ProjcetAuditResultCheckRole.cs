using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Project
{
  

    [Serializable]
    [Table("ProjcetAuditResultCheckRole")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjcetAuditResultCheckRole : FullAuditedEntity<Guid>
    {
        #region 表字段
      

        /// <summary>
        /// 
        /// </summary>
        public Guid CategroyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal DeductionPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }



        #endregion

    }

}
