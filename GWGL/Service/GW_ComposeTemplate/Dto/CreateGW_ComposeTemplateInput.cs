using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace GWGL
{
    [AutoMapTo(typeof(GW_ComposeTemplate))]
    public class CreateGW_ComposeTemplateInput 
    {
        #region 表字段
        /// <summary>
        /// Title
        /// </summary>
        [MaxLength(1000,ErrorMessage = "标题长度必须小于1000")]
        [Required(ErrorMessage = "必须填写标题")]
        public string Title { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [Required(ErrorMessage = "必须填写内容")]
        public string Content { get; set; }


		
        #endregion
    }
}