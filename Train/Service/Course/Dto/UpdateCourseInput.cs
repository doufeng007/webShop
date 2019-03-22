using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace Train
{
    public class UpdateCourseInput : CreateCourseInput
    {
		public Guid FlowId { get; set; }

        public Guid InStanceId { get; set; }
       public bool IsUpdateForChange { get; set; }        
    }
}