using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;

namespace HR
{
    [Serializable]
    [Table("EmployeeAskForLeave")]
    public class EmployeeAskForLeave : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        #region 表字段

        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// BeginTime
        /// </summary>
        [DisplayName(@"BeginTime")]
        [LogColumn("开始时间", true)]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [DisplayName(@"EndTime")]
        [LogColumn("结束时间", true)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [DisplayName(@"Reason")]
        [MaxLength(300)]
        [LogColumn("请假事由", true)]
        public string Reason { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [DisplayName(@"Remark")]
        [MaxLength(200)]
        [LogColumn("情况描述", true)]
        public string Remark { get; set; }

        /// <summary>
        /// TenantId
        /// </summary>
        [DisplayName(@"TenantId")]
        public int? TenantId { get; set; }


        /// <summary>
        /// 时长
        /// </summary>
        [LogColumn("时长", true)]
        public decimal? Hours { get; set; }

        /// <summary>
        /// 事项委托人
        /// </summary>
        [LogColumn("事项委托人", true)]
        public long? RelationUserId { get; set; }

        public long OrgId { get; set; }
        public string PostIds { get; set; }

        

        public string DealWithUsers { get; set; }


        #endregion
    }
}