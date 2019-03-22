using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingGL
{
    public class CreateXZGLMeetingLogisticsRInput
    {

        public Guid LogisticsId { get; set; }


        public string Remark { get; set; }


        [Required(ErrorMessage = "会务后勤必须指定经办人")]
        public long UserId { get; set; }

    }

    public class XZGLMeetingLogisticsROutput : CreateXZGLMeetingLogisticsRInput
    {

        public Guid Id { get; set; }
        public string LogisticsName { get; set; }

        public string UserName { get; set; }
    }


    public class UpdateXZGLMeetingLogisticsRInput : CreateXZGLMeetingLogisticsRInput
    {
        public Guid? Id { get; set; }
    }
}