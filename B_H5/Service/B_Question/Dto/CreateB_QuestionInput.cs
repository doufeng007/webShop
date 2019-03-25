using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_Question))]
    public class CreateB_QuestionInput 
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [MaxLength(200,ErrorMessage = "Content长度必须小于200")]
        [Required(ErrorMessage = "必须填写Content")]
        public string Content { get; set; }


		
        #endregion
    }
}