using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;

namespace IMLib
{
    [AutoMapTo(typeof(IM_InquiryResult))]
    public class CreateIM_InquiryResultInput 
    {
        #region 表字段
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 讨论组Id
        /// </summary>
        public string IM_GroupId { get; set; }

        /// <summary>
        /// 意见征询Id
        /// </summary>
        public Guid InquiryId { get; set; }

        /// <summary>
        /// 回复用户
        /// </summary>
        public long ReplyUserId { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }


		
        #endregion
    }
}