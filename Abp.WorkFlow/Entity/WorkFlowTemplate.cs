using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow.Entity
{
    /// <summary>
    /// 模版定义
    /// </summary>
    [AutoMapTo(typeof(WorkFlowTemplate))]
    public class WorkFlowTemplate : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public TemplateType TemplateType { get; set; }

        //public virtual WorkFlowModel WorkFlowModel { get; set; }
        public Guid WorkFlowModelId { get; set; }
        public string VueTemplate { get; set; }
        /// <summary>
        /// 是否锁定编辑
        /// </summary>
        public bool IsLocked { get; set; }
        /// <summary>
        /// 上次锁定时间
        /// </summary>
        public DateTime? LastLockTime { get; set; }
        /// <summary>
        /// 上次锁定人
        /// </summary>
        public long? LastLockUserId { get; set; }
        /// <summary>
        /// 上次锁定IP
        /// </summary>
        public string LastLockIP { get; set; }
        public int? TenantId { get; set; }
    }
    /// <summary>
    /// 模板修改历史记录
    /// </summary>
    public class WorkFlowTemplateLog : Entity<Guid>
    {
        public string VueTemplate { get; set; }
        public long EditUserId { get; set; }

        public DateTime EditTime { get; set; }
        public Guid TemplateId { get; set; }
        public int VersionNum { get; set; }
        public string LastLockIP { get; set; }
    }
    public enum TemplateType {
        编辑模版=0,
        打印模版=1,
    }
}
