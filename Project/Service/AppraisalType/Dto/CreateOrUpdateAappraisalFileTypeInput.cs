using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class CreateOrUpdateAappraisalFileTypeInput
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }


        public int AppraisalTypeId { get; set; }


        public bool IsPaperFile { get; set; }

        public bool IsMust { get; set; }


        public string AuditRoleIds { get; set; }

        public int Sort { get; set; }
    }
}
