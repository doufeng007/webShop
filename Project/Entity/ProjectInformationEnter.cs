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

    [Table("ProjectInformationEnter")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectInformationEnter : FullAuditedEntity<Guid>
    {
        #region 表字段

      

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectBaseId")]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("UserId")]
        public long UserId { get; set; }



        public Guid TaskId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("InformationEnterType")]
        public int InformationEnterType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }


        public string Content { get; set; }

        #endregion

    }
}