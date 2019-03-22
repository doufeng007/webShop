using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace CWGL
{
    public class UpdateAccountantCourseInput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}