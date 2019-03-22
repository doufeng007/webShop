using System;
using System.Collections.Generic;
using System.Text;

namespace Train
{
    public class RecommendedIndexOutputDto
    {
        /// <summary>
        /// 观看人数
        /// </summary>
        public int WatchPerson { get; set; }
        /// <summary>
        /// 观看次数
        /// </summary>
        public int WatchNumber { get; set; }
        /// <summary>
        /// 课程数
        /// </summary>
        public int CourseCount { get; set; }
        /// <summary>
        /// 课时数
        /// </summary>
        public int CourseTimeCount { get; set; }
        /// <summary>
        /// 人均课时数
        /// </summary>
        public decimal AvgPersonCourseTimeCount { get; set; }
        /// <summary>
        /// 人均课程数
        /// </summary>
        public decimal AvgPersonCourseCount { get; set; }
        /// <summary>
        /// 获得积分
        /// </summary>
        public int CourseScoreCount { get; set; }
        /// <summary>
        /// 人均积分
        /// </summary>
        public decimal AvgCourseScoreCount { get; set; }
    }
}
