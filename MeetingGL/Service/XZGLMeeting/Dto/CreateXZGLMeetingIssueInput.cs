using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingGL
{
    public class CreateXZGLMeetingIssueInput
    {

        public Guid IssueId { get; set; }

        [Required(ErrorMessage ="议题必须指定汇报人")]
        public string UserIds { get; set; }

    }
}