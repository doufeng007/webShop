using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace GWGL
{
    public class UpdateGW_DocumentTypeInput : CreateGW_DocumentTypeInput
    {
		public Guid Id { get; set; }        
    }
}