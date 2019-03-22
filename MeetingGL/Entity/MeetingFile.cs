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
    [Table("MeetingFile")]
    public class MeetingFile : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 会议编号
        /// </summary>
        [DisplayName(@"会议编号")]
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 资料名称
        /// </summary>
        [DisplayName(@"资料名称")]
        [MaxLength(500)]
        [Required]
        public string FileName { get; set; }

        /// <summary>
        /// 提供人员
        /// </summary>
        [DisplayName(@"提供人员")]
        public long UserId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName(@"状态")]
        public int Status { get; set; }


        #endregion
    }
}