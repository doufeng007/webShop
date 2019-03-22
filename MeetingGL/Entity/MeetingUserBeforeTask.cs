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
    [Table("MeetingUserBeforeTask")]
    public class MeetingUserBeforeTask : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 会议编号
        /// </summary>
        [DisplayName(@"会议编号")]
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        [DisplayName(@"任务类型")]
        public int TaskType { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        [DisplayName(@"人员")]
        public long UserId { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DisplayName(@"Remark")]
        [MaxLength(1000)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName(@"状态")]
        public int Status { get; set; }

        /// <summary>
        /// DealWithUsers
        /// </summary>
        [DisplayName(@"DealWithUsers")]
        [MaxLength(500)]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// CopyForUsers
        /// </summary>
        [DisplayName(@"CopyForUsers")]
        [MaxLength(500)]
        public string CopyForUsers { get; set; }


        #endregion
    }
}