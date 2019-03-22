using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;

namespace IMLib
{
    public class CreateIM_InquiryInput 
    {
        #region 表字段
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


        public List<Guid> MessageIds { get; set; } = new List<Guid>();


		
        #endregion
    }
}