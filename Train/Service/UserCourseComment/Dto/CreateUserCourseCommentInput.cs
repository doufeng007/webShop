using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Train
{
    [AutoMapTo(typeof(UserCourseComment))]
    public class CreateUserCourseCommentInput 
    {
        #region 表字段

        /// <summary>
        /// 课程编号
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        [MaxLength(500,ErrorMessage = "评价内容长度必须小于500")]
        [Required(ErrorMessage = "必须填写评价内容")]
        public string Comment { get; set; }


		
        #endregion
    }
}