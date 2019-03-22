using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Supply.Entity
{
   // [Table("SupplyBackMain")]
   /// <summary>
   /// 退还申请主表
   /// </summary>
    public class SupplyBackMain : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 0:草稿 1：行政处理中  -1:处理完成
        /// </summary>
        public int Status { get; set; }


    }
}
