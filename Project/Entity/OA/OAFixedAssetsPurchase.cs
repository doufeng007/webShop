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
    [Table("OAFixedAssetsPurchase")]
    public class OAFixedAssetsPurchase : FullAuditedEntity<Guid>
    {
        #region 表字段


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
        [DisplayName("ApplyType")]
        public string ApplyType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ApplyTypeCode")]
        public string ApplyTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FeeSource")]
        public string FeeSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FeeSourceCode")]
        public string FeeSourceCode { get; set; }

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
        [DisplayName("Reason")]
        public string Reason { get; set; }

       

        #endregion


    }
}
