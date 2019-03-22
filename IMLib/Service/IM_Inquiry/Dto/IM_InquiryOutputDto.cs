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
    [AutoMapFrom(typeof(IM_Inquiry))]
    public class IM_InquiryOutputDto 
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
}
