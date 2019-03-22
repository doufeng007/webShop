using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace MeetingGL
{
    public class UpdateMeetingUserBeforeTaskInput : CreateMeetingUserBeforeTaskInput
    {
		public Guid FlowId { get; set; }
       public Guid InStanceId { get; set; }
       public bool IsUpdateForChange { get; set; }

        public List<UpdateXZGLMeetingFileInput> FileList = new List<UpdateXZGLMeetingFileInput>();


        public List<UpdateXZGLMeetingLogisticsRInput> LogList = new List<UpdateXZGLMeetingLogisticsRInput>();

    }
}