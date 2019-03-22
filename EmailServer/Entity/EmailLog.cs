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

namespace EmailServer
{
    [Serializable]
    [Table("EmailLog")]
    public class EmailLog : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// To
        /// </summary>
        [DisplayName(@"To")]
        public string To { get; set; }

        /// <summary>
        /// CC
        /// </summary>
        [DisplayName(@"CC")]
        public string CC { get; set; }

        /// <summary>
        /// From
        /// </summary>
        [DisplayName(@"From")]
        [MaxLength(200)]
        public string From { get; set; }

        /// <summary>
        /// Subject
        /// </summary>
        [DisplayName(@"Subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Body
        /// </summary>
        [DisplayName(@"Body")]
        public string Body { get; set; }


        #endregion
    }
}