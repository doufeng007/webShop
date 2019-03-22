using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace XZGL
{
    public class UpdateXZGLCompanyInput : CreateXZGLCompanyInput
    {
		public Guid Id { get; set; }        
    }
}