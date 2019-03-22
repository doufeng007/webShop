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
    /// 客户档案
    /// </summary>
    [Table("OACustomer")]
    public class OACustomer : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Tel { get; set; }

        public string Phone { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public string Web { get; set; }

        public string Des { get; set; }

        public int Status { get; set; }

        public string AuditUser { get; set; }

        public Guid? OAContractId { get; set; }
    }
}
