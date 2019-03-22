using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [Table("OATenderEnemy")]
    public class OATenderEnemy : FullAuditedEntity<Guid>
    {
        public Guid ProjectId { get; set; }

        public string ProjectType { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 对手情况 json-OATenderEnemyItem
        /// </summary>
        public string Enemy { get; set; }

        public int Status { get; set; }

        public string AuditUser { get; set; }
    }

   
}
