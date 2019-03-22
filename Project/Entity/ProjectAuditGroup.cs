using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;

namespace Project

{

    [Serializable]
    [TableNameAtribute("项目评审组")]
    [Table("ProjectAuditGroup")]
    public class ProjectAuditGroup: FullAuditedEntity<Guid>
    {
        public string Name { get; set; }


        public string Description { get; set; }
    }
}
