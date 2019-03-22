using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingGL
{
    public class SubmitRecordInput
    {

        public Guid Id { get; set; }


        public DateTime StartTime { get; set; }


        public DateTime EndTime { get; set; }

        public string Record { get; set; }

        public string CopyForUsers { get; set; }


        /// <summary>
        ///实际与会人员
        /// </summary>
        public string RealAttendeeUsers { get; set; }

        /// <summary>
        /// 实际参会嘉宾
        /// </summary>
        public string RealMeetingGuest { get; set; }

        /// <summary>
        /// 缺席人
        /// </summary>
        public string AbsentUser { get; set; }


        public List<SubmitRecordIssueResultInput> IssueResults { get; set; } = new List<SubmitRecordIssueResultInput>();

        public List<CreateOrUpdateMeetingIssueInput> NewIsseuList { get; set; } = new List<CreateOrUpdateMeetingIssueInput>();
    }

    public class SubmitRecordIssueResultInput
    {
        public Guid IssueId { get; set; }

        public bool HasPass { get; set; }
    }


    
}