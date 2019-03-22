using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingGL
{
    public class CreateXZGLMeetingFileInput
    {

        public string Name { get; set; }

        [Required(ErrorMessage = "会议资料必须指定提供人")]
        public long UserId { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();

    }


    public class XZGLMeetingFileOutput : CreateXZGLMeetingFileInput
    {
        public Guid Id { get; set; }


        public string UserName { get; set; }

        /// <summary>
        /// 准备状态
        /// </summary>
        public string ReadStatus { get; set; }


    }


    public class UpdateXZGLMeetingFileInput : CreateXZGLMeetingFileInput
    {
        public Guid? Id { get; set; }
    }
}