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

    [Table("ProjectAuditMemberResult")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectAuditMemberResult : FullAuditedEntity<Guid>
    {
        #region 表字段


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Pid")]
        public Guid Pid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Files")]
        public string Files { get; set; }

        [DisplayName("CJZFiles")]
        public string CJZFiles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Remark")]
        public string Remark { get; set; }


        public decimal? AuditAmount { get; set; }


        #endregion

    }
}