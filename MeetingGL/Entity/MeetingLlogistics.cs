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

namespace MeetingGL
{
    [Serializable]
    [Table("MeetingLlogistics")]
    public class MeetingLlogistics : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 事项
        /// </summary>
        [DisplayName(@"事项")]
        [MaxLength(500)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 会议类型
        /// </summary>
        [DisplayName(@"会议类型")]
        public Guid? MeetingTypeId { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        [DisplayName(@"经办人")]
        public long UserId { get; set; }


        #endregion
    }
}