using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace MeetingGL
{
    public class UpdateMeetingLlogisticsInput : CreateMeetingLlogisticsInput
    {
		public Guid Id { get; set; }        
    }
}