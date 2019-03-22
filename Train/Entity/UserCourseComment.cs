using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;

namespace Train
{
    [Serializable]
    [Table("UserCourseComment")]
    public class UserCourseComment : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 用户编号
        /// </summary>
        [DisplayName(@"用户编号")]
        public long UserId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        [DisplayName(@"课程编号")]
        public Guid CourseId { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        [DisplayName(@"评价内容")]
        [MaxLength(500)]
        [Required]
        public string Comment { get; set; }


        #endregion
    }
}