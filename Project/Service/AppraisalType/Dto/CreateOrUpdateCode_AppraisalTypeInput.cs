using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class CreateOrUpdateCode_AppraisalTypeInput
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public int ParentId { get; set; }

        public int Sort { get; set; }
    }
}
