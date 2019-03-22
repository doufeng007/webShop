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
    [Table("MeetingLogisticsRelation")]
    public class MeetingLogisticsRelation : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 会议编号
        /// </summary>
        [DisplayName(@"会议编号")]
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 后勤编号
        /// </summary>
        [DisplayName(@"后勤编号")]
        public Guid LogisticsId { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        [DisplayName(@"经办人")]
        public long UserId { get; set; }

        /// <summary>
        /// 需求描述
        /// </summary>
        [DisplayName(@"需求描述")]
        [MaxLength(1000)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName(@"状态")]
        public int Status { get; set; }


        #endregion
    }
}