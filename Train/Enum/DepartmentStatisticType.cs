using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Train.Enum
{
    public enum DepartmentStatisticType
    {
        /// <summary>
        /// 观看人数
        /// </summary>
        [Description("观看人数")] WatchPerson = 1,
        /// <summary>
        /// 观看次数
        /// </summary>
        [Description("观看次数")] WatchNumber = 2,
        /// <summary>
        /// 课程数
        /// </summary>
        [Description("课程数")] CourseCount = 3,
        /// <summary>
        /// 课时数
        /// </summary>
        [Description("课时数")] CourseTimeCount = 4,
        /// <summary>
        /// 人均课时数
        /// </summary>
        [Description("人均课时数")] AvgPersonCourseTimeCount = 5,
        /// <summary>
        /// 人均课程数
        /// </summary>
        [Description("人均课程数")] AvgPersonCourseCount = 6,
        /// <summary>
        /// 获得积分
        /// </summary>
        [Description("获得积分")] CourseScoreCount = 7,
        /// <summary>
        /// 人均积分
        /// </summary>
        [Description("人均积分")] AvgCourseScoreCount = 8,
    }
}
