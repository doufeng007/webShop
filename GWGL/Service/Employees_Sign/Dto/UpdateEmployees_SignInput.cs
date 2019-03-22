using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace GWGL
{
    public class UpdateEmployees_SignInput : CreateEmployees_SignInput
    {
		public Guid Id { get; set; }        
    }
}