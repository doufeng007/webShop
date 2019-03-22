using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    /// <summary>
    /// 简历推荐表
    /// </summary>
    [Serializable]
    [Table("CommendResume")]
    public class CommendResume : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public string Reason { get; set; }

        public string Source { get; set; }

        public string OnlineUrl { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Guid? Job { get; set; }
        
        public int Status { get; set; }
        public int? TenantId { get; set; }
    }
}
