using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace Train
{
    public class UpdateUserCourseRecordNumberInput : CreateUserCourseRecordNumberInput
    {
		public Guid Id { get; set; }        
    }
}