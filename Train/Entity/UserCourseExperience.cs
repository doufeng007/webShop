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
    [Table("UserCourseExperience")]
    public class UserCourseExperience : FullAuditedEntity<Guid>, IMayHaveTenant
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
        /// 心得体会编号
        /// </summary>
        [DisplayName(@"心得体会编号")]
        public string ExperienceId { get; set; }


        /// <summary>
        /// 提交心得体会人员
        /// </summary>
        [DisplayName(@"提交心得体会人员")]
        public string SubmitUsers { get; set; }
        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [DisplayName(@"流程查阅人员")]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int? Status { get; set; }

        /// <summary>
        /// 抄送查阅人员
        /// </summary>
        [DisplayName(@"抄送查阅人员")]
        public string CopyForUsers { get; set; }
        
        #endregion
    }
}