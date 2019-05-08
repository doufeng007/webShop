using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_Message))]
    public class CreateB_MessageInput 
    {
        #region 表字段
        /// <summary>
        /// Title
        /// </summary>
        [MaxLength(500,ErrorMessage = "Title长度必须小于500")]
        [Required(ErrorMessage = "必须填写Title")]
        public string Title { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        [MaxLength(100,ErrorMessage = "Code长度必须小于100")]
        [Required(ErrorMessage = "必须填写Code")]
        public string Code { get; set; }

        /// <summary>
        /// BusinessType
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public B_H5MesagessType BusinessType { get; set; }

        /// <summary>
        /// BusinessId
        /// </summary>
        public Guid BusinessId { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [MaxLength(500,ErrorMessage = "Content长度必须小于500")]
        public string Content { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Status { get; set; }


        /// <summary>
        /// 缺货描述
        /// </summary>
        public string LessRemark { get; set; }


        public string StatusTitle { get; set; }


        public long UserId { get; set; }
        #endregion
    }
}