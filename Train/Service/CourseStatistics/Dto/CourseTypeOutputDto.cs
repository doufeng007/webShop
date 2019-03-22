using System;
using System.Collections.Generic;
using System.Text;

namespace Train
{
    public class CourseTypeOutputDto
    {
        /// <summary>
        /// 课程id
        /// </summary>
        public Guid CourseId { get; set; }
        /// <summary>
        /// 排名
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 课程名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 学习人数
        /// </summary>
        public int CourseUser { get; set; }
        /// <summary>
        /// 学习课时数
        /// </summary>
        public int _CourseUserTime { get; set; }
        public double CourseUserTime { get; set; }
        /// <summary>
        /// 心得体会数
        /// </summary>
        public int Experience { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int Comment { get; set; }
    }
}
