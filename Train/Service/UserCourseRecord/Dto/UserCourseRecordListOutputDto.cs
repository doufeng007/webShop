using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Train.Enum;

namespace Train
{
    [AutoMapFrom(typeof(UserCourseRecord))]
    public class UserCourseRecordListOutputDto
    {
        /// <summary>
        /// 课程编号
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// 课程封面
        /// </summary>
        public string FaceUrl { get; set; }
        /// <summary>
        /// 课程链接
        /// </summary>
        public string CourseLink { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程简介
        /// </summary>
        public string CourseIntroduction { get; set; }

        /// <summary>
        /// 课程时长
        /// </summary>
        public int LearnTime { get; set; }
        /// <summary>
        /// 我的观看时长
        /// </summary>
        public int MyLearnTime { get; set; }

        /// <summary>
        /// 课程分值
        /// </summary>
        public int LearnScore { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsComplate { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ComplateTime { get; set; }
        /// <summary>
        /// 观看比例
        /// </summary>
        public decimal ViewingRatio { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 学习类型
        /// </summary>
        public CourseLearnType LearnType { get; set; }

        /// <summary>
        /// 课程专业性
        /// </summary>
        public bool IsSpecial { get; set; }
        /// <summary>
        /// 课程文件类型
        /// </summary>
        public CourseFileType CourseFileType { get; set; }
    }
}
