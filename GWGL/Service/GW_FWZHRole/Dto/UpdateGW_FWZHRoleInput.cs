using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace GWGL
{
    public class UpdateGW_FWZHRoleInput : CreateGW_FWZHRoleInput
    {
		public Guid Id { get; set; }        
    }
}