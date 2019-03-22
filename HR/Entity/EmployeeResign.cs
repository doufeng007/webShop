using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    /// <summary>
    /// 员工离职申请
    /// </summary>
    [Table("EmployeeResign")]
    public class EmployeeResign : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 员工信息
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 离职类别
        /// </summary>
        public EmployeeResignType Type { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public EmployeeResignStatus Status { get; set; }
        /// <summary>
        /// 流程参与人员
        /// </summary>
        public string DealWithUsers { get; set; }
        public long OrgId { get; set; }

        public string PostIds { get; set; }

    }
}
