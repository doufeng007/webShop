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
    [Table("OATask")]
    public class OATask : FullAuditedEntity<Guid>
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
        [DisplayName("Code")]
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Priority")]
        public string Priority { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("PriorityCode")]
        public string PriorityCode { get; set; }



        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }



        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ValUser")]
        public long ValUser { get; set; }


        public string ValUser1 { get; set; }

        public string ExecutorUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Summary")]
        public string Summary { get; set; }




        /// <summary>
        /// 
        /// </summary>
        [DisplayName("File")]
        public string File { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Status")]
        public int Status { get; set; }


        public string NotifyUsers { get; set; }

        public int CreateByBusinessRole { get; set; }


        #endregion

    }

    [Table("OATaskUser")]
    public class OATaskUser : FullAuditedEntity<Guid>
    {
        #region 表字段


        public Guid OATaskId { get; set; }

        public long UserId { get; set; }



        #endregion
    }
}
