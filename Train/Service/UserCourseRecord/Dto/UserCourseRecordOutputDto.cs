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
    public class UserCourseRecordOutputDto 
    {

        /// <summary>
        /// 课程编号
        /// </summary>
        public Guid CourseId { get; set; }
        
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

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
        /// 课程时长
        /// </summary>
        public int LearnTime { get; set; }

        /// <summary>
        /// 我的学习时长
        /// </summary>
        public int MyLearnTime { get; set; }

        /// <summary>
        /// 我的点赞情况
        /// </summary>
        public CourseFavorState FavorState { get; set; }
        /// <summary>
        /// 总的赞
        /// </summary>
        public int AllFavor { get; set; }
        /// <summary>
        /// 总的踩
        /// </summary>
        public int AllDiss { get; set; }
        /// <summary>
        /// 课程文件类型
        /// </summary>
        public CourseFileType CourseFileType { get; set; }
        /// <summary>
        /// 课程链接
        /// </summary>
        public string CourseLink { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
