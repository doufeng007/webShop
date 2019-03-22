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

namespace MeetingGL
{
    [Table("XZGLMeetingRoom")]
    public class XZGLMeetingRoom : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// 会议室名称
        /// </summary>
        [DisplayName(@"会议室名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 会议室位置
        /// </summary>
        [DisplayName(@"会议室位置")]
        [MaxLength(200)]
        public string Address { get; set; }


        /// <summary>
        /// 是否启用
        /// </summary>
        [DisplayName(@"是否启用")]
        public bool IsEnable { get; set; }


        public MeetingRoomSizeType MeetingRoomSizeType { get; set; }

        public int Number { get; set; }

        public long? UserId { get; set; }


        /// <summary>
        ///  0 空置 1 使用中
        /// </summary>
        public int BookingStatus { get; set; }

        #endregion
    }
}