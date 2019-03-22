using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace MeetingGL
{
    public class UpdateMeetingTypeBaseInput : CreateMeetingTypeBaseInput
    {
		public Guid Id { get; set; }        
    }
}