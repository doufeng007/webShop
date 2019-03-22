using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply.Entity
{
    public class SupplyApplyMain : FullAuditedEntity<Guid>
    {
        public int Status { get; set; }

        public bool IsImportant { get; set; }


        /// <summary>
        /// 处理申购状态
        /// </summary>
        public int DoPurchaseStatus { get; set; }


    }
}
