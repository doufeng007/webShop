using System;
using System.Collections.Generic;
using System.Text;

namespace Train
{
    public class CourseExperienceDetailOutputDto
    {
        /// <summary>
        /// 培训id
        /// </summary>
        public Guid ExperienceId { get; set; }

        /// <summary>
        /// 参训人员
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsComplate { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }
    }
}
