using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// 员工生日
    /// </summary>
    [Table("OABirthday")]
    public class OABirthday : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 员工
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 祝福语
        /// </summary>
        public string Words { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
    }
}
