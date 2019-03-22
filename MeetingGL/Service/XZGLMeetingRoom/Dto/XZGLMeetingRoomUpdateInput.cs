using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingGL
{
    [AutoMapTo(typeof(XZGLMeetingRoom))]
    public class XZGLMeetingRoomUpdateInput:CreateXZGLMeetingRoomInput
    {
        public int? TenantId { get; set; }

        public Guid Id { get; set; }
    }
}