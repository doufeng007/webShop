using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project

{
    [AutoMapFrom(typeof(OAMeeting))]
    public class OAMeetingDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Adress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long HostUser { get; set; }


        public string HostUser_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? DepartmentId { get; set; }

        public string DepartmentId_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long NoteUser { get; set; }

        public string NoteUser_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OtherUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }


        public string ParticipateUser { get; set; }


        public string NotifyUsers { get; set; }

        public List<CreateOrUpdateOAMeetingUserInput> Users { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }

        public string ParticipateUser_Name { get; set; }



        public OAMeetingDto()
        {
            this.Users = new List<CreateOrUpdateOAMeetingUserInput>();
            this.FileList = new List<GetAbpFilesOutput>();
        }

    }


}
