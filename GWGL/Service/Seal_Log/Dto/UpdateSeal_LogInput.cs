using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace GWGL
{
    public class UpdateSeal_LogInput : CreateSeal_LogInput
    {
		public Guid Id { get; set; }        
    }
}