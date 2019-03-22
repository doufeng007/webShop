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
    [Table("OAFixedAssetsRepair")]
    public class OAFixedAssetsRepair : FullAuditedEntity<Guid>
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
        [DisplayName("ApplyUserId")]
        public string ApplyUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ApplyDate")]
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FAId")]
        public Guid FAId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FAName")]
        public string FAName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Spec")]
        public string Spec { get; set; }


        public decimal PreCost { get; set; }

        public decimal Cost { get; set; }

        public DateTime? RequestComplateDate { get; set; }

        public DateTime? ComplateDate { get; set; }
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

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Remark")]
        public string Remark { get; set; }


        public string Reason { get; set; }

        #endregion

    }
    public enum OAFixedAssetsRepairStatus
    {
        维修申请 = 0,
        维修完成 = 1,
        维修开始 = 2,
    }
}
