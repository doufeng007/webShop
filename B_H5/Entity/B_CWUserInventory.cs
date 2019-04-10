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
    [Table("B_CWUserInventory")]
    public class B_CWUserInventory : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 库存数量
        /// </summary>
        [DisplayName(@"Count")]
        public int Count { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        [DisplayName(@"CategroyId")]
        public Guid CategroyId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// LessCount
        /// </summary>
        [DisplayName(@"LessCount")]
        public int LessCount { get; set; }


        #endregion
    }
}