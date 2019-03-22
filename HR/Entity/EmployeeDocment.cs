using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    /// <summary>
    /// 员工档案
    /// </summary>
    public class EmployeeDocment : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 员工信息
        /// </summary>
        public Guid EmployeeId { get; set; }


    }
}
