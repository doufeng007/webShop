using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace MeetingGL
{
    public class UpdateMeetingUserInput
    {
       public Guid InStanceId { get; set; }
       public bool IsUpdateForChange { get; set; }


        /// <summary>
        /// 回执状态
        /// </summary>
        public ReturnReceiptStatus ReturnReceiptStatus { get; set; }


        /// <summary>
        /// 请假备注
        /// </summary>
        public string AskForLeaveRemark { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();


    }
}