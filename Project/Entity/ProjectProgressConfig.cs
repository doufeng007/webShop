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

namespace Project
{
    /// <summary>
    /// 项目进度比例配置
    /// </summary>
    [Table("ProjectProgressConfig")]
    public class ProjectProgressConfig : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// ProjectBaseId
        /// </summary>
        [DisplayName(@"ProjectBaseId")]
        public Guid? ProjectBaseId { get; set; }

        /// <summary>
        /// FirstAuditKey
        /// </summary>
        [DisplayName(@"FirstAuditKey")]
        public int FirstAuditKey { get; set; }

        /// <summary>
        /// JiliangKey
        /// </summary>
        [DisplayName(@"JiliangKey")]
        public int JiliangKey { get; set; }

        /// <summary>
        /// JijiaKey
        /// </summary>
        [DisplayName(@"JijiaKey")]
        public int JijiaKey { get; set; }

        /// <summary>
        /// SelfAuditKey
        /// </summary>
        [DisplayName(@"SelfAuditKey")]
        public int SelfAuditKey { get; set; }

        /// <summary>
        /// SecondAuditKey
        /// </summary>
        [DisplayName(@"SecondAuditKey")]
        public int SecondAuditKey { get; set; }

        /// <summary>
        /// LastAuditKey
        /// </summary>
        [DisplayName(@"LastAuditKey")]
        public int LastAuditKey { get; set; }


        #endregion
    }
}

