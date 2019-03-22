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
    [Table("OAFixedAssetsReturn")]
    public class OAFixedAssetsReturn : FullAuditedEntity<Guid>
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
        [DisplayName("UseApplyId")]
        public Guid UseApplyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("UserId")]
        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ReturnDate")]
        public DateTime ReturnDate { get; set; }

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

     

        #endregion






    }
}
