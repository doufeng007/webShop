using System;
using System.Collections.Generic;
using System.Text;
using NPOI.SS.Formula.Eval;

namespace Train
{
    public class DepartmentUserOutputDto
    {
        /// <summary>
        /// 课程名
        /// </summary>
        public string CourseName { get; set; }
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
