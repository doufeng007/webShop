using System;
using System.Collections.Generic;
using System.Text;
using Train.Enum;

namespace Train
{
    public class CourseExperienceSummaryOutputDto
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        public Guid CourseId { get; set; }
        /// <summary>
        /// 课程名
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 已上传心得体会人员
        /// </summary>
        public int SubmitUser { get; set; }

        /// <summary>
        /// 未上传心得体会人员
        /// </summary>
        public int UnSubmitUser { get; set; }

        /// <summary>
        /// 课程创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否已发送流程
        /// </summary>
        public bool IsSendFlow { get; set; }

        public string LearnUser { get; set; }

        public CourseLearnType LearnType { get; set; }
    }
}
