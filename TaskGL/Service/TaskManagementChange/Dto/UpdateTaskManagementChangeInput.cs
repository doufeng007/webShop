using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace TaskGL
{
    public class UpdateTaskManagementChangeInput : CreateTaskManagementChangeInput
    {
		public Guid Id { get; set; }        
    }
}