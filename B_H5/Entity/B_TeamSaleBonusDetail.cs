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
    [Table("B_TeamSaleBonusDetail")]
    public class B_TeamSaleBonusDetail : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// Pid
        /// </summary>
        [DisplayName(@"Pid")]
        public Guid Pid { get; set; }

        /// <summary>
        /// MaxSale
        /// </summary>
        [DisplayName(@"MaxSale")]
        public decimal MaxSale { get; set; }

        /// <summary>
        /// MinSale
        /// </summary>
        [DisplayName(@"MinSale")]
        public decimal MinSale { get; set; }

        /// <summary>
        /// Scale
        /// </summary>
        [DisplayName(@"Scale")]
        public decimal Scale { get; set; }


        #endregion
    }
}