using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using Train.Enum;

namespace Train
{
    public class UpdateUserCourseRecordInput
    {
		public Guid CourseId { get; set; }
        public CourseFavorState FavorState { get; set; }
    }
}