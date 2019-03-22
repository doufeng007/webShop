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

    [Table("ProjectAuditStop")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectAuditStop : FullAuditedEntity<Guid>
    {
        #region 表字段


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectId")]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Files")]
        public string Files { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Remark")]
        public string Remark { get; set; }


        /// <summary> 
        ///状态
        /// </summary>
        public int Status { get; set; }


        public string RelieveRemark { get; set; }


        public int DelayDay { get; set; }

        public bool? CreateByLeader { get; set; }

        //public StopTypeEnum StopType { get; set; }


        public string DealWithUsers { get; set; }


        #endregion

    }
}