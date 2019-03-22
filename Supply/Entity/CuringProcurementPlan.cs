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

namespace Supply
{
    [Table("CuringProcurementPlan")]
    public class CuringProcurementPlan : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// MainId
        /// </summary>
        [DisplayName(@"MainId")]
        public Guid MainId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DisplayName(@"Name")]
        [MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [DisplayName(@"Version")]
        [MaxLength(30)]
        public string Version { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [DisplayName(@"Number")]
        [MaxLength(300)]
        public string Number { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        [DisplayName(@"Unit")]
        [MaxLength(100)]
        public string Unit { get; set; }

        /// <summary>
        /// Money
        /// </summary>
        [DisplayName(@"Money")]
        public string Money { get; set; }

        /// <summary>
        /// Des
        /// </summary>
        [DisplayName(@"Des")]
        public string Des { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [DisplayName(@"Type")]
        public string Type { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DisplayName(@"Remark")]
        [MaxLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }


        public int BusinessType { get; set; }


        #endregion
    }
}