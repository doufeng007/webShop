using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply.Entity
{
    /// <summary>
    /// 用品维修
    /// </summary>
    public class SupplyRepair : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 0:申请中  1：已处理 -2驳回
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 用品id
        /// </summary>
        public Guid SupplyId { get; set; }
        /// <summary>
        /// 维修时限
        /// </summary>
        public DateTime RepairEndTime { get; set; }
        /// <summary>
        /// 故障描述
        /// </summary>
        public string Des { get; set; }

        public bool IsImportant { get; set; }

        /// <summary>
        /// 维修结果
        /// </summary>
        public RepairResultEnum RepairResult { get; set; }


        /// <summary>
        /// 验收记录
        /// </summary>
        public string CheckRemark { get; set; }

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason { get; set; }

        public Guid UserSupplyId { get; set; }


        public Guid? SupplierId { get; set; }


        public string SupplierName { get; set; }
    }
}
