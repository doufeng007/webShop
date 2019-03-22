using System;
using System.Collections.Generic;
using System.Text;

namespace Train
{
    public class TrainScoreOutputDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        public string PostName { get; set; }
        public string DepartmentName { get; set; }
        /// <summary>
        /// 课程数
        /// </summary>
        public int ExperienceScore { get; set; }
        /// <summary>
        /// 课时数
        /// </summary>
        public int JoinScore { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
    }
}
