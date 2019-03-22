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
    [Table("EmployeeTrainingSystemUnitPosts")]
    public class EmployeeTrainingSystemUnitPosts : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// 制度编号
        /// </summary>
        [DisplayName(@"制度编号")]
        public Guid SysId { get; set; }

        /// <summary>
        /// 部门岗位编号
        /// </summary>
        [DisplayName(@"部门岗位编号")]
        public Guid PortsId { get; set; }


        #endregion
    }
}