using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;

namespace IMLib
{
    [AutoMapFrom(typeof(IM_InquiryResult))]
    public class IM_InquiryResultOutputDto 
    {
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

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


		
    }
}
