using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project
{
    [Table("ConstructionOrganizations")]
    public class ConstructionOrganizations : FullAuditedEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string ContactUser { get; set; }

        [StringLength(50)]
        public string ContactTel { get; set; }
    }
}