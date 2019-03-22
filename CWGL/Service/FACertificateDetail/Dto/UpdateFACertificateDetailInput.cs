using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace CWGL
{
    public class UpdateFACertificateDetailInput : FACertificateDetail
    {
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }
    }
}