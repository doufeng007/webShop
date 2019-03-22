using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Train.Enum;

namespace Train
{
    [AutoMapTo(typeof(Course))]
    public class CreateCourseInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 课程名称
        /// </summary>
        [MaxLength(50,ErrorMessage = "课程名称长度必须小于50")]
        [Required(ErrorMessage = "必须填写课程名称")]
        public string CourseName { get; set; }

        /// <summary>
        /// 课程类别
        /// </summary>
        public Guid CourseType { get; set; }

        /// <summary>
        /// 课程链接
        /// </summary>
        public string CourseLink { get; set; }

        /// <summary>
        /// 课程文件类型
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public CourseFileType CourseFileType { get; set; }

        /// <summary>
        /// 课程时长
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int LearnTime { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        public string Recommend { get; set; }

        /// <summary>
        /// 推荐语
        /// </summary>
        [MaxLength(200,ErrorMessage = "推荐语长度必须小于200")]
        public string RecommendWords { get; set; }

        /// <summary>
        /// 课程简介
        /// </summary>
        [MaxLength(200,ErrorMessage = "课程简介长度必须小于200")]
        public string CourseIntroduction { get; set; }

        /// <summary>
        /// 开启心得体会
        /// </summary>
        public bool IsExperience { get; set; }

        /// <summary>
        /// 指派人员
        /// </summary>
        public string LearnUser { get; set; }

        /// <summary>
        /// 学习类型
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public CourseLearnType LearnType { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? ComplateTime { get; set; }

        /// <summary>
        /// 课程专业性
        /// </summary>
        public bool IsSpecial { get; set; }

        /// <summary>
        /// 课程页数
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int FilePage { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 抄送查阅人员
        /// </summary>
        public string CopyForUsers { get; set; }


        /// <summary>
        /// 课程封面
        /// </summary>
        public List<GetAbpFilesOutput> CourseCoverFile { get; set; } = new List<GetAbpFilesOutput>();

        /// <summary>
        /// 课程文件
        /// </summary>
        public List<GetAbpFilesOutput> CourseFile { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
}