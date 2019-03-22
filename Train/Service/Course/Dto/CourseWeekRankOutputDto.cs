using System;
using System.Collections.Generic;
using System.Text;

namespace Train.Service.Course.Dto
{
    /// <summary>
    /// 课程排行榜(每周)
    /// </summary>
    public class CourseWeekRankOutputDto
    {
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 分值
        /// </summary>
        public int Score { get; set; }
    }
}
