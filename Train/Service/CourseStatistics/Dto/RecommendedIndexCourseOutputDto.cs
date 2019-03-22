using System;
using System.Collections.Generic;
using System.Text;

namespace Train
{
    public class RecommendedIndexCourseOutputDto
    {
        /// <summary>
        /// 上传课程资源数
        /// </summary>
        public int UpResource { get; set; }

        /// <summary>
        /// 上传资源课时数
        /// </summary>
        public int UpResourceTime { get; set; }

        /// <summary>
        /// 学习人数
        /// </summary>
        public int LearnUser { get; set; }

        /// <summary>
        /// 学习时长
        /// </summary>
        public int LearnTime { get; set; }

        /// <summary>
        /// 心得体会
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int Comment { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int Favor { get; set; }

        /// <summary>
        /// 踩数
        /// </summary>
        public int Diss { get; set; }
    }
}
