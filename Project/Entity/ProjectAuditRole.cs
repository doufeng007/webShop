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
    [Table("ProjectAuditRole")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectAuditRole : FullAuditedEntity
    {
        #region 表字段

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("AppraisalTypeId")]
        public int AppraisalTypeId { get; set; }


        public int Sort { get; set; }

        #endregion

    }
}
