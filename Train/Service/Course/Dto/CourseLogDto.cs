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
using Train.Enum;

namespace Train
{
	[AutoMapFrom(typeof(Course))]
    public class CourseLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        [LogColumn(@"课程名称", IsLog = true)]
        public string CourseName { get; set; }

        /// <summary>
        /// 课程类别
        /// </summary>
        [LogColumn(@"课程类别", IsLog = true)]
        public Guid CourseType { get; set; }

        /// <summary>
        /// 课程链接
        /// </summary>
        [LogColumn(@"课程链接", IsLog = true)]
        public string CourseLink { get; set; }

        /// <summary>
        /// 课程文件类型
        /// </summary>
        [LogColumn(@"课程文件类型", IsLog = true)]
        public CourseFileType CourseFileType { get; set; }

        /// <summary>
        /// 课程时长
        /// </summary>
        [LogColumn(@"课程时长", IsLog = true)]
        public int LearnTime { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        [LogColumn(@"推荐人", IsLog = true)]
        public string Recommend { get; set; }

        /// <summary>
        /// 推荐语
        /// </summary>
        [LogColumn(@"推荐语", IsLog = true)]
        public string RecommendWords { get; set; }

        /// <summary>
        /// 课程简介
        /// </summary>
        [LogColumn(@"课程简介", IsLog = true)]
        public string CourseIntroduction { get; set; }

        /// <summary>
        /// 开启心得体会
        /// </summary>
        [LogColumn(@"开启心得体会", IsLog = true)]
        public bool IsExperience { get; set; }

        /// <summary>
        /// 指派人员
        /// </summary>
        [LogColumn(@"指派人员", IsLog = true)]
        public string LearnUser { get; set; }

        /// <summary>
        /// 学习类型
        /// </summary>
        [LogColumn(@"学习类型", IsLog = true)]
        public CourseLearnType LearnType { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [LogColumn(@"完成时间", IsLog = true)]
        public DateTime? ComplateTime { get; set; }

        /// <summary>
        /// 课程专业性
        /// </summary>
        [LogColumn(@"课程专业性", IsLog = true)]
        public bool IsSpecial { get; set; }

        /// <summary>
        /// 课程页数
        /// </summary>
        [LogColumn(@"课程页数", IsLog = true)]
        public int FilePage { get; set; }

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