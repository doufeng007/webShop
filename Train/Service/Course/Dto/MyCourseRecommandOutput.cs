using System;
using System.Collections.Generic;
using System.Text;
using Abp.File;
using Abp.WorkFlow;
using Train.Enum;

namespace Train
{
    public class MyCourseRecommandOutput : BusinessWorkFlowListOutput
    {

        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 推荐语
        /// </summary>
        public string RecommendWords { get; set; }

        /// <summary>
        /// 课程链接
        /// </summary>
        public string CourseLink { get; set; }

        /// <summary>
        /// 课程文件类型
        /// </summary>
        public CourseFileType CourseFileType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 课程文件
        /// </summary>
        public List<GetAbpFilesOutput> CourseFile { get; set; } = new List<GetAbpFilesOutput>();


        public List<WorkFlowTaskCommentDto> CommentList { get; set; }

        public MyCourseRecommandOutput()
        {
            this.CommentList = new List<WorkFlowTaskCommentDto>();
        }
    }
}
