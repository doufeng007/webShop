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
    [Table("EmployeeFinanceSalaryBill")]
    public class EmployeeFinanceSalaryBill : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int? Status { get; set; }

        /// <summary>
        /// 提示发工资时间
        /// </summary>
        [DisplayName(@"提示发工资时间")]
        public DateTime SalaryBillTime { get; set; }


        #endregion
    }
}