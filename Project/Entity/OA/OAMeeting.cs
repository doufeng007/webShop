using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Project
{
    [Table("OAMeeting")]
    public class OAMeeting : FullAuditedEntity<Guid>
    {
        #region 表字段


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Adress")]
        public string Adress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("StartDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EndDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HostUser")]
        public long HostUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("DepartmentId")]
        public long? DepartmentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("NoteUser")]
        public long NoteUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OtherUsers")]
        public string OtherUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("File")]
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Content")]
        public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Status")]
        public int Status { get; set; }

        public string ParticipateUser { get; set; }


        public string NotifyUsers { get; set; }





        #endregion
    }


    [Table("OAMeetingUser")]
    public class OAMeetingUser : FullAuditedEntity<Guid>
    {
        #region 表字段


        public Guid OAMeetingId { get; set; }

        public long UserId { get; set; }



        #endregion
    }
}
