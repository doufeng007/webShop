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
    [AutoMapFrom(typeof(UserCourseRecordDetail))]
    public class UserCourseRecordDetailOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// 当前修习时长
        /// </summary>
        public int LearningTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


		
    }
}
