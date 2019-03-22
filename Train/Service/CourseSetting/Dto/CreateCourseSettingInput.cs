using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System;

namespace Train
{
    [AutoMapTo(typeof(CourseSetting))]
    public class CreateCourseSettingInput 
    {
        #region 表字段
        /// <summary>
        /// 课程设置内容
        /// </summary>
        public string Content { get; set; }


		
        #endregion
    }
}