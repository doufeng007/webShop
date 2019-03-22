using System;
using System.Collections.Generic;
using System.Text;

namespace Train.Service.UserCourseRecord.Dto
{
    public class UserCourseStatisticsOutputDto
    {
        /// <summary>
        /// 我的课程已完成数
        /// </summary>
        public int MyComplateSourceScore { get; set; }

        /// <summary>
        /// 我的课程总数
        /// </summary>
        public int MyAllSourceScore { get; set; }

        /// <summary>
        /// 必修课程已完成数
        /// </summary>
        public int RequiredComplateSourceScore { get; set; }

        /// <summary>
        /// 必修课程数
        /// </summary>
        public int RequiredAllSourceScore { get; set; }

        /// <summary>
        /// 自选课程已完成数
        /// </summary>
        public int ElectiveComplateSourceScore { get; set; }
        /// <summary>
        /// 自选课程总数
        /// </summary>
        public int ElectiveAllSourceScore { get; set; }
        /// <summary>
        /// 学习时长
        /// </summary>
        public int LearnTime { get; set; }

        /// <summary>
        /// 本周排名
        /// </summary>
        public int ThisWeekRank { get; set; }

    }
}
