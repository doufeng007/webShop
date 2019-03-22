using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace MeetingGL
{
    public class UpdateMeetingRoomUseInfoInput : CreateMeetingRoomUseInfoInput
    {
		public Guid Id { get; set; }        
    }
}