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

namespace Train
{
    [Serializable]
    [Table("TrainLeave")]
    public class TrainLeave : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 用户编号
        /// </summary>
        [DisplayName(@"用户编号")]
        public long UserId { get; set; }

        /// <summary>
        /// 培训编号
        /// </summary>
        [DisplayName(@"培训编号")]
        public Guid TrainId { get; set; }

        /// <summary>
        /// 请假类型
        /// </summary>
        [DisplayName(@"请假类型")]
        public Guid? LevelType { get; set; }

        /// <summary>
        /// 请假事由
        /// </summary>
        [DisplayName(@"请假事由")]
        [MaxLength(500)]
        public string Reason { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [DisplayName(@"流程查阅人员")]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }

        /// <summary>
        /// 抄送查阅人员
        /// </summary>
        [DisplayName(@"抄送查阅人员")]
        public string CopyForUsers { get; set; }

        /// <summary>
        /// 请假开始时间
        /// </summary>
        [DisplayName(@"请假开始时间")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 请假结束时间
        /// </summary>
        [DisplayName(@"请假结束时间")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 请假天数
        /// </summary>
        [DisplayName(@"请假天数")]
        public decimal? Day { get; set; }


        #endregion
    }
}