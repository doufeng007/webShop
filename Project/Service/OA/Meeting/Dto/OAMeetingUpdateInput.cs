using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Castle.Components.DictionaryAdapter;
using Abp.File;

namespace Project
{
    [AutoMapTo(typeof(OAMeeting))]
    public class OAMeetingUpdateInput
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


        /// <summary>
        /// 
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long NoteUser { get; set; }


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


        public List<GetAbpFilesOutput> FileList { get; set; }



        public OAMeetingUpdateInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }
    }




}
