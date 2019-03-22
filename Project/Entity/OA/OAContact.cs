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
    /// 通讯录
    /// </summary>
    [Table("OAContact")]
    public class OAContact: FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 关联用户
        /// </summary>
        public long? UserId { get; set; }

        public string CompanyName { get; set; }

        public string DepartMent { get; set; }

        public string Duty { get; set; }

        public string OfficeTel { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string OfficeAddress { get; set; }

        public string QQ { get; set; }

        public string Wechat { get; set; }

        public string Name { get; set; }

        public bool Sex { get; set; }

        public int Sort { get; set; }

        public string Des { get; set; }

        public string NickName { get; set; }


    }
}
