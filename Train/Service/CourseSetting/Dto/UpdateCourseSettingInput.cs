using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Train.Enum;

namespace Train
{
    public class CourseSet
    {
        public CourseSet()
        {
            RequiredCommonly = new CourseSetScore();
            RequiredMajor = new CourseSetScore();
            ElectiveCommonly = new CourseSetScore();
            ElectiveMajor = new CourseSetScore();
        }

        /// <summary>
        /// 观看比例
        /// </summary>
        [Required(ErrorMessage = "请填写观看比例")]
        [Range(0, 100, ErrorMessage = "观看比例只能在0-100之间")]
        public decimal ViewingRatio { get; set; }

        /// <summary>
        /// 一般必修课设置
        /// </summary>

        public CourseSetScore RequiredCommonly { get; set; }
        /// <summary>
        /// 专业必修课设置
        /// </summary>
        public CourseSetScore RequiredMajor { get; set; }
        /// <summary>
        /// 一般选修课设置
        /// </summary>
        public CourseSetScore ElectiveCommonly { get; set; }
        /// <summary>
        /// 专业选修课设置
        /// </summary>
        public CourseSetScore ElectiveMajor { get; set; }
        /// <summary>
        /// 图文换算--一般课程页码
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "页码不能小于0")]
        public int CommonlyPage { get; set; }
        /// <summary>
        /// 图文换算--一般课程课时
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "课时不能小0")]
        public int CommonlyClassHour { get; set; }
        /// <summary>
        /// 图文换算--专业课程页码
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "页码不能小于0")]
        public int MajorPage { get; set; }
        /// <summary>
        /// 图文换算--专业课程课时
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "课时不能小于0")]
        public int MajorClassHour { get; set; }
    }

    public static class CourseSetExtensions
    {
        public static CourseSetScore GetSetVal(this CourseSet courseSet, CourseLearnType type, bool isSpecial)
        {
            if (type == CourseLearnType.Must || type == CourseLearnType.MustAll)
            {
                return isSpecial ? courseSet.RequiredMajor : courseSet.RequiredCommonly;
            }
            return isSpecial ? courseSet.ElectiveMajor : courseSet.ElectiveCommonly;
        }
    }
    public class CourseSetScore
    {
        /// <summary>
        /// 课时积分
        /// </summary>
        public int ClassHourScore { get; set; }
        /// <summary>
        /// 采纳心得体会积分
        /// </summary>
        public int ExperienceScore { get; set; }

        /// <summary>
        /// 单次评论积分
        /// </summary>
        public int CommentScore { get; set; }

    }
}