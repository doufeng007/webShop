using System;
using System.Collections.Generic;
using System.Text;

namespace Train
{
    public class CourseUserOutputDto
    {
        /// <summary>
        /// 学员名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 最后观看时间
        /// </summary>
        public DateTime LastWatchTime { get; set; }
        /// <summary>
        /// 观看次数
        /// </summary>
        public int WatchNumber { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int LearnState { get; set; }
    }
}
