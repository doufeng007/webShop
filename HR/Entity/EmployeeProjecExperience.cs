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

namespace HR
{
    [Serializable]
    [Table("EmployeeProjecExperience")]
    public class EmployeeProjecExperience : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 项目名称
        /// </summary>
        [DisplayName(@"项目名称")]
        [MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 担任职位
        /// </summary>
        [DisplayName(@"担任职位")]
        [MaxLength(20)]
        public string Position { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DisplayName(@"开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DisplayName(@"结束时间")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 项目内容
        /// </summary>
        [DisplayName(@"项目内容")]
        [MaxLength(500)]
        public string Content { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        [DisplayName(@"员工编号")]
        public Guid EmployeeId { get; set; }


        #endregion
    }
}