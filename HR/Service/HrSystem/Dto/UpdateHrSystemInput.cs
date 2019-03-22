using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace HR
{
    public class UpdateHrSystemInput : CreateHrSystemInput
    {
		public Guid Id { get; set; }        
    }
}