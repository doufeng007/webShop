using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Train.Enum
{
    public enum CourseTypeStatisticType
    {
        /// <summary>
        /// 上传课程资源数
        /// </summary>
        [Description("上传课程资源数")] UpResource = 1,

        /// <summary>
        /// 上传资源课时数
        /// </summary>
        [Description("上传资源课时数")] UpResourceTime = 2,

        /// <summary>
        /// 学习人数
        /// </summary>
        [Description("学习人数")] LearnUser = 3,

        /// <summary>
        /// 学习时长
        /// </summary>
        [Description("学习时长")] LearnTime = 4,

        /// <summary>
        /// 心得体会
        /// </summary>
        [Description("心得体会")] Experience = 5,

        /// <summary>
        /// 积分
        /// </summary>
        [Description("积分")] Score = 6,

        /// <summary>
        /// 评论数
        /// </summary>
        [Description("评论数")] Comment = 7,

        /// <summary>
        /// 点赞数
        /// </summary>
        [Description("点赞数")] Favor = 8,

        /// <summary>
        /// 踩数
        /// </summary>
        [Description("踩数")] Diss = 9,
    }
}
