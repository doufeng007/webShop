using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Train
{
    [AutoMapTo(typeof(UserCourseRecordNumber))]
    public class CreateUserCourseRecordNumberInput 
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


		
        #endregion
    }
}