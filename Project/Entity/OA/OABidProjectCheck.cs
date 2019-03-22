using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [Table("OABidProjectCheck")]
    public class OABidProjectCheck : FullAuditedEntity<Guid>
    {
        #region 表字段

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Code")]
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectId")]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectName")]
        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ApplyUser")]
        public string ApplyUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ApplyDate")]
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[DisplayName("Amount")]
        //public string Amount { get; set; }


        public string Participant { get; set; }


        public string Summary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("File")]
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Status")]
        public int Status { get; set; }



        #endregion




    }

    public enum OABidProjectCheckStatus
    {
        新增 = 0,
        项目勘察完成 = 1,
        正在项目勘察 = 2,
    }
}
