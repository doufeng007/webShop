using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;
using Train.Enum;

namespace Train
{
    [AutoMapFrom(typeof(Course))]
    public class CourseOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程类别
        /// </summary>
        public Guid CourseType { get; set; }
        /// <summary>
        /// 课程类别名
        /// </summary>
        public string CourseTypeName { get; set; }

        /// <summary>
        /// 课程链接
        /// </summary>
        public string CourseLink { get; set; }

        /// <summary>
        /// 课程文件类型
        /// </summary>
        public CourseFileType CourseFileType { get; set; }

        /// <summary>
        /// 课程时长
        /// </summary>
        public int LearnTime { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        public string Recommend { get; set; }

        /// <summary>
        /// 推荐语
        /// </summary>
        public string RecommendWords { get; set; }

        /// <summary>
        /// 课程简介
        /// </summary>
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
        public int FilePage { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

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
    }
}
