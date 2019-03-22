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

namespace HR
{
    [Serializable]
    [Table("Daily")]
    public class Daily : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// Department
        /// </summary>
        [DisplayName(@"Department")]
        [MaxLength(20)]
        public string Department { get; set; }

        /// <summary>
        /// Personnel
        /// </summary>
        [DisplayName(@"Personnel")]
        [MaxLength(20)]
        public string Personnel { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [DisplayName(@"Content")]
        public string Content { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        [DisplayName(@"StartTime")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [DisplayName(@"EndTime")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// OverState
        /// </summary>
        [DisplayName(@"OverState")]
        [MaxLength(20)]
        public string OverState { get; set; }

        /// <summary>
        /// Note
        /// </summary>
        [DisplayName(@"Note")]
        public string Note { get; set; }


        #endregion
    }
}