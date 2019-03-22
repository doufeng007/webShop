using System;
using System.Collections.Generic;
using System.Text;

namespace Train
{
    public class DepartmentOutputDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 排名
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 课程数
        /// </summary>
        public int CourseNumber { get; set; }
        /// <summary>
        /// 课时数
        /// </summary>
        public int CourseTimeNumber { get; set; }
        /// <summary>
        /// 观看次数
        /// </summary>
        public int WatchNumber { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }

        public string ChargeLeader { get; set; }
    }
}
