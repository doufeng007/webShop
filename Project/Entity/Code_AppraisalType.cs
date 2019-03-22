using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{

    public class Code_AppraisalType : FullAuditedEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }

        

        [MaxLength(50)]
        public string Code { get; set; }


        public int ParentId { get; set; }

        public int Sort { get; set; }

        public Code_AppraisalType() { }
    }
}
