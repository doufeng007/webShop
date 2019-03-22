using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Abp.File;

namespace IMLib
{
    [AutoMapFrom(typeof(IM_InquiryResult))]
    public class IM_InquiryResultListOutputDto
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

    public class InquiryResultOutput
    {
        public Guid Id { get; set; }
        public DateTime ReceiveTime { get; set; }

        public string SenderName { get; set; }

        public string Comment { get; set; }
        public string ReceiveName { get; set; }

        public long? ReplyUserId { get; set; }

        public DateTime CompletedTime1 { get; set; }

        public Guid InquiryId { get; set; }


        public string MessageType { get; set; }


        public List<GetAbpFilesOutput> Files { get; set; } = new List<GetAbpFilesOutput>();
    }
}
