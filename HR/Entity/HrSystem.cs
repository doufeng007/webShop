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
using HR.Enum;

namespace HR
{
    [Serializable]
    [Table("HrSystem")]
    public class HrSystem : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName(@"标题")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName(@"内容")]
        public string Content { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName(@"类型")]
        public HrSystemType TypeId { get; set; }

        /// <summary>
        /// 人员权限
        /// </summary>
        [DisplayName(@"人员权限")]
        public string UserIds { get; set; }

        /// <summary>
        /// 是否全公司
        /// </summary>
        [DisplayName(@"是否全公司")]
        public bool IsAll { get; set; }

        /// <summary>
        /// 部门权限
        /// </summary>
        [DisplayName(@"部门权限")]
        public string OrgIds { get; set; }

        /// <summary>
        /// 人员权限
        /// </summary>
        [DisplayName(@"人员权限")]
        public string UserNames { get; set; }

        /// <summary>
        /// 部门权限
        /// </summary>
        [DisplayName(@"部门权限")]
        public string OrgNames { get; set; }


        #endregion
    }
}