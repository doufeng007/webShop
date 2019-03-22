using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Train
{
    [AutoMapTo(typeof(UserCourseRecordDetail))]
    public class CreateUserCourseRecordDetailInput 
    {
        #region 表字段
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
        [Range(0, int.MaxValue,ErrorMessage="修习时长不得小于0")]
        public int LearningTime { get; set; }


		
        #endregion
    }
}