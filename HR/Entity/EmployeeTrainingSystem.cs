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
using HR.Enum;

namespace HR
{
    [Table("EmployeeTrainingSystem")]
    public class EmployeeTrainingSystem : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// 制度标题
        /// </summary>
        [DisplayName(@"制度标题")]
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 制度内容
        /// </summary>
        [DisplayName(@"制度内容")]
        public string Contents { get; set; }

        /// <summary>
        /// 制度类型
        /// </summary>
        [DisplayName(@"制度类型")]
        public TrainingSystemType Type { get; set; }
        #endregion
    }
}