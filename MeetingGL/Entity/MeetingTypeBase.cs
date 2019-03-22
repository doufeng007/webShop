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
    [Table("MeetingTypeBase")]
    public class MeetingTypeBase : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName(@"名称")]
        [MaxLength(500)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 是否启用回执
        /// </summary>
        [DisplayName(@"是否启用回执")]
        public bool ReturnReceiptEnable { get; set; }

        /// <summary>
        /// 是否启用提醒
        /// </summary>
        [DisplayName(@"是否启用提醒")]
        public bool WarningEnable { get; set; }

        /// <summary>
        /// 会前提醒时间数
        /// </summary>
        [DisplayName(@"会前提醒时间数")]
        public int? WarningDateNumber { get; set; }

        /// <summary>
        /// 会前提示时间类型
        /// </summary>
        [DisplayName(@"会前提示时间类型")]
        public WraningDataTypeEnum? WraningDataType { get; set; }

        /// <summary>
        /// 会前提醒方式
        /// </summary>
        [DisplayName(@"会前提醒方式")]
        public WraingTypeEnum? WraingType { get; set; }

        /// <summary>
        /// 签到开始时间
        /// </summary>
        [DisplayName(@"签到开始时间")]
        public int SignTime1 { get; set; }

        /// <summary>
        /// 签到结束时间
        /// </summary>
        [DisplayName(@"签到结束时间")]
        public int SignTime2 { get; set; }

        /// <summary>
        /// 签到规则
        /// </summary>
        [DisplayName(@"签到规则")]
        public string SignType { get; set; }

        /// <summary>
        /// 状态  0 禁用  1 启用
        /// </summary>
        [DisplayName(@"状态")]
        public int Status { get; set; }


        #endregion
    }
}