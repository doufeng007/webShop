using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;

namespace B_H5
{
    [Serializable]
    [Table("B_Categroy")]
    public class B_Categroy : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// Name
        /// </summary>
        [DisplayName(@"Name")]
        [MaxLength(500)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// P_Id
        /// </summary>
        [DisplayName(@"P_Id")]
        public Guid? P_Id { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [DisplayName(@"Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        [DisplayName(@"Unit")]
        [MaxLength(200)]
        [Required]
        public string Unit { get; set; }

        /// <summary>
        /// Tag
        /// </summary>
        [DisplayName(@"Tag")]
        [MaxLength(200)]
        public string Tag { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DisplayName(@"Remark")]
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }

        /// <summary>
        /// 一级商品类别属性  来源于数据字典，  进提货、 直购、试装
        /// </summary>
        public FirestLevelCategroyProperty FirestLevelCategroyPropertyId {get;set;}


        #endregion
    }
}