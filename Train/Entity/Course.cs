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
using Train.Enum;
using ZCYX.FRMSCore;

namespace Train
{
    [Serializable]
    [Table("Course")]
    public class Course : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// 课程名称
        /// </summary>
        [DisplayName(@"课程名称")]
        [MaxLength(50)]
        [Required]
        public string CourseName { get; set; }

        /// <summary>
        /// 课程类别
        /// </summary>
        [DisplayName(@"课程类别")]
        public Guid CourseType { get; set; }

        /// <summary>
        /// 课程链接
        /// </summary>
        [DisplayName(@"课程链接")]
        public string CourseLink { get; set; }

        /// <summary>
        /// 课程文件类型
        /// </summary>
        [DisplayName(@"课程文件类型")]
        public CourseFileType CourseFileType { get; set; }

        /// <summary>
        /// 课程时长
        /// </summary>
        [DisplayName(@"课程时长")]
        public int LearnTime { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        [DisplayName(@"推荐人")]
        [MaxLength(20)]
        public string Recommend { get; set; }

        /// <summary>
        /// 推荐语
        /// </summary>
        [DisplayName(@"推荐语")]
        [MaxLength(200)]
        public string RecommendWords { get; set; }

        /// <summary>
        /// 课程简介
        /// </summary>
        [DisplayName(@"课程简介")]
        [MaxLength(200)]
        public string CourseIntroduction { get; set; }

        /// <summary>
        /// 开启心得体会
        /// </summary>
        [DisplayName(@"开启心得体会")]
        public bool IsExperience { get; set; }

        /// <summary>
        /// 指派人员
        /// </summary>
        [DisplayName(@"指派人员")]
        [Required]
        public string LearnUser { get; set; }

        /// <summary>
        /// 学习类型
        /// </summary>
        [DisplayName(@"学习类型")]
        public CourseLearnType LearnType { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [DisplayName(@"完成时间")]
        public DateTime? ComplateTime { get; set; }

        /// <summary>
        /// 课程专业性
        /// </summary>
        [DisplayName(@"课程专业性")]
        public bool IsSpecial { get; set; }

        /// <summary>
        /// 课程页数
        /// </summary>
        [DisplayName(@"课程页数")]
        public int FilePage { get; set; }

        /// <summary>
        /// 课程软删除
        /// </summary>
        [DisplayName(@"课程软删除")]
        public bool IsDelCourse { get; set; }
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