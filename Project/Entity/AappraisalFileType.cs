using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [Table("AappraisalFileType")]
    public class AappraisalFileType : FullAuditedEntity
    {
        public int AppraisalTypeId { get; set; }



        [MaxLength(100)]
        public string Name { get; set; }

        
        public bool IsPaperFile { get; set; }

        public int Sort { get; set; }

        public bool IsMust { get; set; }

        public string AuditRoleIds { get; set; }

        public AappraisalFileType() { }


         
    }
}
