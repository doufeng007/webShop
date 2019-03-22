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

namespace ZCYX.FRMSCore.Application
{
    [Serializable]
    [Table("Employees_Sign")]
    public class Employees_Sign : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// UserId
        /// </summary>
        [DisplayName(@"UserId")]
        public long UserId { get; set; }

        /// <summary>
        /// SignType
        /// </summary>
        [DisplayName(@"SignType")]
        public GW_EmployeesSignTypelEnmu SignType { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public GW_EmployeesSignStatusEnmu Status { get; set; }


        #endregion
    }
}