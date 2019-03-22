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

namespace CWGL
{
    [Serializable]
    [Table("AccountantCourse")]
    public class AccountantCourse : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName(@"名称")]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        [DisplayName(@"父节点")]
        public Guid? Pid { get; set; }

        /// <summary>
        /// parent_left
        /// </summary>
        [DisplayName(@"parent_left")]
        public int parent_left { get; set; }

        /// <summary>
        /// parent_right
        /// </summary>
        [DisplayName(@"parent_right")]
        public int parent_right { get; set; }


        #endregion
    }
}