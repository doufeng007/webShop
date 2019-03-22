using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace CWGL
{
    public class UpdateCWGLReceivableInput : CreateCWGLReceivableInput
    {
        public Guid Id { get; set; }
        public Guid FlowId { get; set; }
       public Guid InStanceId { get; set; }
       public bool IsUpdateForChange { get; set; }        
    }
}