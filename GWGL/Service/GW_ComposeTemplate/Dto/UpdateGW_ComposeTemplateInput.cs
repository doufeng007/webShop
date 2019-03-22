using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace GWGL
{
    public class UpdateGW_ComposeTemplateInput : CreateGW_ComposeTemplateInput
    {
		public Guid Id { get; set; }        
    }
}