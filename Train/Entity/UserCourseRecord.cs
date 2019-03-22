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
    [Table("UserCourseRecord")]
    public class UserCourseRecord : FullAuditedEntity<Guid>, IMayHaveTenant
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
        /// 是否点赞
        /// </summary>
        [DisplayName(@"是否点赞")]
        public bool? IsFavor { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        [DisplayName(@"是否完成")]
        public bool? IsComplete { get; set; }

        /// <summary>
        /// 用户课程总分
        /// </summary>
        [DisplayName(@"用户课程总分")]
        public int? Score { get; set; }
        /// <summary>
        /// 用户修习总时长
        /// </summary>
        [DisplayName("用户修习总时长")]
        public int? LearnTime { get; set; }
        #endregion
    }
}