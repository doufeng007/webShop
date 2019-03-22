using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace IMLib
{
    public class UpdateIM_InquiryInput : IM_Inquiry
    {
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }
    }
}