using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project
{
    [Table("ReplyUnit")]
    public class ReplyUnit : FullAuditedEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        
        public int Sort { get; set; }


    }
}