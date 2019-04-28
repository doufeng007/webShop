using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_Notice))]
    public class CreateB_NoticeInput 
    {
        #region 表字段
        /// <summary>
        /// Title
        /// </summary>
        [MaxLength(500,ErrorMessage = "Title长度必须小于500")]
        [Required(ErrorMessage = "必须填写Title")]
        public string Title { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public NoticeStatusEnum Status { get; set; }


		
        #endregion
    }
}