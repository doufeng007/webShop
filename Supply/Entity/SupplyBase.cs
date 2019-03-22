using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Supply
{

    [Table("Supply")]
    public class SupplyBase : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public decimal Money { get; set; }

        public int Type { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }


        public string Code { get; set; }

        public DateTime? ProductDate { get; set; }


        public DateTime? ExpiryDate { get; set; }

        public string Unit { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime? PutInDate { get; set; }

    }
}
