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
using Abp.AutoMapper;

namespace Train
{
	[AutoMapFrom(typeof(UserCourseExperience))]
    public class UserCourseExperienceLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [LogColumn(@"用户编号", IsLog = true)]
        public long UserId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        [LogColumn(@"课程编号", IsLog = true)]
        public Guid CourseId { get; set; }

        /// <summary>
        /// 心得体会编号
        /// </summary>
        [LogColumn(@"心得体会编号", IsLog = true)]
        public Guid? ExperienceId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [LogColumn(@"流程查阅人员", IsLog = true)]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [LogColumn(@"Status", IsLog = true)]
        public int? Status { get; set; }

        /// <summary>
        /// 抄送查阅人员
        /// </summary>
        [LogColumn(@"抄送查阅人员", IsLog = true)]
        public string CopyForUsers { get; set; }


        #endregion
    }
}