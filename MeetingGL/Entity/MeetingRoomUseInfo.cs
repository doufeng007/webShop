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
    [Table("MeetingRoomUseInfo")]
    public class MeetingRoomUseInfo : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 会议室编号
        /// </summary>
        [DisplayName(@"会议室编号")]
        public Guid MeetingRoomId { get; set; }

        /// <summary>
        /// 业务id
        /// </summary>
        [DisplayName(@"业务id")]
        public Guid BusinessId { get; set; }

        [DisplayName(@"业务名称")]
        public string BusinessName { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [DisplayName(@"业务类型")]
        public MeetingRoomUseBusinessType BusinessType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DisplayName(@"开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DisplayName(@"结束时间")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName(@"状态")]
        public int Status { get; set; }


        #endregion
    }
}