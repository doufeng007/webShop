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
    [Table("MeetingPeriodRule")]
    public class MeetingPeriodRule : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// 会议编号
        /// </summary>
        [DisplayName(@"会议编号")]
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 会议开始时间
        /// </summary>
        [DisplayName(@"会议开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        [DisplayName(@"会议结束时间")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 生效开始时间
        /// </summary>
        [DisplayName(@"生效开始时间")]
        public DateTime ActiveStartTime { get; set; }

        /// <summary>
        /// 生效结束时间
        /// </summary>
        [DisplayName(@"生效结束时间")]
        public DateTime ActiveEndTime { get; set; }

        /// <summary>
        /// 重复模式
        /// </summary>
        [DisplayName(@"重复模式")]
        public PeriodType PeriodType { get; set; }

        /// <summary>
        /// 重复参数1
        /// </summary>
        [DisplayName(@"重复参数1")]
        public int PeriodNumber1 { get; set; }

        /// <summary>
        /// 重复参数2
        /// </summary>
        [DisplayName(@"重复参数2")]
        public int PeriodNumber2 { get; set; }

        /// <summary>
        /// 重复参数3
        /// </summary>
        [DisplayName(@"重复参数3")]
        public int PeriodNumber3 { get; set; }

        /// <summary>
        /// 提前创建时间类型
        /// </summary>
        [DisplayName(@"提前创建时间类型")]
        public PreTimeType PreTimeType { get; set; }

        /// <summary>
        /// 提前创建时间数
        /// </summary>
        [DisplayName(@"提前创建时间数")]
        public int PreTimeNum { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName(@"状态")]
        public PeriodRuleStatus Status { get; set; }

        /// <summary>
        /// 下次创建时间
        /// </summary>
        public DateTime? NextCreateTime { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 创建会议时间是否提前至少1天
        /// </summary>
        public bool HasAdvanceLessOneDay { get; set; }


        #endregion
    }
}