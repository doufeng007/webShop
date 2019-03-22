using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace CWGL
{
    public class UpdateCWGLCredentialInput : CreateCWGLCredentialInput
    {
        public Guid Id { get; set; }
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }
    }
}