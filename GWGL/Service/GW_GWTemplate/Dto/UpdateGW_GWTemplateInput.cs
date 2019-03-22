using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace GWGL
{
    public class UpdateGW_GWTemplateInput : CreateGW_GWTemplateInput
    {
		public Guid Id { get; set; }        
    }
}