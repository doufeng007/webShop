using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    /// <summary>
    /// 人事申请为新员工创建帐号
    /// </summary>
    [Table("CreateAccount")]
    public class CreateAccount : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public long Department { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public Guid Post { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? JoinTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        public int? TenantId { get; set; }
    }
}
