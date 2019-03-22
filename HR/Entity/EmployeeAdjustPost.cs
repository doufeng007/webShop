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

namespace HR
{
    [Table("EmployeeAdjustPost")]
    public class EmployeeAdjustPost : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 原部门
        /// </summary>
        [DisplayName(@"原部门")]
        public long OriginalDepId { get; set; }

        /// <summary>
        /// 原岗位
        /// </summary>
        [DisplayName(@"原岗位")]
        public Guid OriginalPostId { get; set; }

        /// <summary>
        /// 情况说明
        /// </summary>
        [DisplayName(@"情况说明")]
        public string Remark { get; set; }

        /// <summary>
        /// 调入部门
        /// </summary>
        [DisplayName(@"调入部门")]
        public long AdjustDepId { get; set; }

        /// <summary>
        /// 申请职位
        /// </summary>
        [DisplayName(@"申请职位")]
        public Guid AdjustPostId { get; set; }

        /// <summary>
        /// DealWithUsers
        /// </summary>
        [DisplayName(@"DealWithUsers")]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int? Status { get; set; }

        public string WorkflowAdjsutDepId { get; set; }
        #endregion
    }
}