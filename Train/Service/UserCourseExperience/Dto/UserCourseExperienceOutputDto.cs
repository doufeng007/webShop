using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;

namespace Train
{
    [AutoMapFrom(typeof(UserCourseExperience))]
    public class UserCourseExperienceOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 抄送查阅人员
        /// </summary>
        public string CopyForUsers { get; set; }
        /// <summary>
        /// 我的心得体会批示
        /// </summary>
        public string MyExperienceApproval { get; set; }
        /// <summary>
        /// 获取心得体会
        /// </summary>
        public List<CourseExperienceDetailOutputDto> ExperienceList { get; set; }

        /// <summary>
        /// 心得体会流程评论列表
        /// </summary>
        public List<TrainUserExperienceOrganListOutputDto> ExperienceCommentList { get; set; }
    }
}
