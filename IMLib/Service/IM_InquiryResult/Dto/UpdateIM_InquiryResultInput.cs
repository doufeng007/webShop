using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace IMLib
{
    public class UpdateIM_InquiryResultInput : IM_InquiryResult
    {
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }
    }
}