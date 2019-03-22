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

    [Table("ProjectAuditMember")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectAuditMember:FullAuditedEntity<Guid>
    {
        #region 表字段



        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectBaseId")]
        public Guid ProjectBaseId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("UserAuditRole")]
        public int UserAuditRole { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Is_Cooperation")]
        public bool IsCooperation { get; set; }

        public string FlowId { get; set; }

        /// <summary>
        /// 分派工作描述
        /// </summary>
        public string WorkDes { get; set; }

        /// <summary>
        /// 分派工作时间
        /// </summary>
        public int? WorkDays { get; set; }
        /// <summary>
        /// 允许查看的文件ids
        /// </summary>
        public string AappraisalFileTypes { get; set; }

        //public string FinishItems { get; set; }

        public Guid? GroupId { get; set; }

        /// <summary>
        /// 绩效占比 数值*100
        /// </summary>
        public int? Percentes { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Des { get; set; }
        #endregion

    }
}