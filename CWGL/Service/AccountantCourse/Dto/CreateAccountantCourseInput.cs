using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;

namespace CWGL
{
    [AutoMapTo(typeof(AccountantCourse))]
    public class CreateAccountantCourseInput 
    {
        #region 表字段
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public Guid Pid { get; set; }

		
        #endregion
    }
}