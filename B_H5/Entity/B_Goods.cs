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
    [Table("B_Goods")]
    public class B_Goods : FullAuditedEntity<Guid>, IMayHaveTenant
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
        /// CategroyId
        /// </summary>
        [DisplayName(@"CategroyId")]
        public Guid? CategroyId { get; set; }


        public Guid CategroyIdP { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [DisplayName(@"Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Pirce1
        /// </summary>
        [DisplayName(@"Pirce1")]
        public decimal Pirce1 { get; set; }

        /// <summary>
        /// Price2
        /// </summary>
        [DisplayName(@"Price2")]
        public decimal Price2 { get; set; }


        public string Code { get; set; }


        public string ModeType { get; set; }



        public string Spe { get; set; }

        public Guid UnitId { get; set; }


        public string UnitName { get; set; }


        public GoodStatusEnum Status { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Inventory { get; set; }

        #endregion
    }
}