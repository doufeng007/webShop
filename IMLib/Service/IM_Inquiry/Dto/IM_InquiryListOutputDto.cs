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
    [AutoMapFrom(typeof(IM_Inquiry))]
    public class IM_InquiryListOutputDto
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
        /// 讨论组名
        /// </summary>
        public string IM_GroupName { get; set; }

        /// <summary>
        /// 待办Id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }


    public class InquiryOutput : FlowInquiryOutput
    {
        public Guid? Id { get; set; }
        public int Index { get; set; }

        public InquiryType InquiryType { get; set; }

        
    }

    public enum InquiryType
    {
        待办征询 = 1,
        讨论组征询 = 2,
    }
}
