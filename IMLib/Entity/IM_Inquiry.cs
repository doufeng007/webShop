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

namespace IMLib
{
    [Serializable]
    [Table("IM_Inquiry")]
    public class IM_Inquiry : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 讨论组Id
        /// </summary>
        [DisplayName(@"讨论组Id")]
        [MaxLength(100)]
        public string IM_GroupId { get; set; }

        /// <summary>
        /// 讨论组名
        /// </summary>
        [DisplayName(@"讨论组名")]
        [MaxLength(500)]
        public string IM_GroupName { get; set; }

        /// <summary>
        /// 待办Id
        /// </summary>
        [DisplayName(@"待办Id")]
        public Guid TaskId { get; set; }


        #endregion
    }
}