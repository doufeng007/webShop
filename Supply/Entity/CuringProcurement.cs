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
    [Table("CuringProcurement")]
    public class CuringProcurement : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// Code
        /// </summary>
        [DisplayName(@"Code")]
        [MaxLength(30)]
        public string Code { get; set; }

        /// <summary>
        /// NeedMember
        /// </summary>
        [DisplayName(@"NeedMember")]
        [MaxLength(30)]
        public string NeedMember { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [DisplayName(@"Type")]
        [MaxLength(300)]
        public string Type { get; set; }

        /// <summary>
        /// ExecuteSummary
        /// </summary>
        [DisplayName(@"ExecuteSummary")]
        [MaxLength(100)]
        public string ExecuteSummary { get; set; }

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


        #endregion
    }
}