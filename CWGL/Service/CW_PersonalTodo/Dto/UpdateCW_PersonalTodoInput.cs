using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace CWGL
{
    public class UpdateCW_PersonalTodoInput : CreateCW_PersonalTodoInput
    {
		public Guid Id { get; set; }        
    }
}