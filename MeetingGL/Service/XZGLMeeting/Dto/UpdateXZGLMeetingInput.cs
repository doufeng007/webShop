using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace MeetingGL
{
    public class UpdateXZGLMeetingInput : CreateXZGLMeetingInput
    {
        public Guid Id { get; set; }
        public bool IsUpdateForChange { get; set; }


        public new UpdateMeetingPeriodRuleInput MeetingPeriodRule { get; set; } = new UpdateMeetingPeriodRuleInput();

        public new List<UpdateXZGLMeetingFileInput> FileList { get; set; } = new List<UpdateXZGLMeetingFileInput>();

        public new List<UpdateXZGLMeetingLogisticsRInput> LogisticsList { get; set; } = new List<UpdateXZGLMeetingLogisticsRInput>();
    }
}