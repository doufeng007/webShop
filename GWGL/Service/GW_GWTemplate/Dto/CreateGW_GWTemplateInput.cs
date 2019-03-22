using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace GWGL
{
    [AutoMapTo(typeof(GW_GWTemplate))]
    public class CreateGW_GWTemplateInput 
    {
        #region 表字段
        /// <summary>
        /// Title
        /// </summary>
        [MaxLength(500,ErrorMessage = "标题长度必须小于500")]
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