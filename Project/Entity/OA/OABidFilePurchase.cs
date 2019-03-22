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
    [Table("OABidFilePurchase")]
    public class OABidFilePurchase : FullAuditedEntity<Guid>
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
        [DisplayName("Amount")]
        public decimal Amount { get; set; }

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

    public enum OABidFilePurchaseStatus
    {
        新增 = 0,
        购买招标文件完成 = 1,
        正在购买招标文件 = 2,
    }
}
