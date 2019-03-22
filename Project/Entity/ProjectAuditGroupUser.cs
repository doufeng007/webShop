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
    [TableNameAtribute("项目评审用户关联表")]
    [Table("ProjectAuditGroupUser")]
    public class ProjectAuditGroupUser : FullAuditedEntity<Guid>
    {
        public Guid GroupId { get; set; }

        public long UserId { get; set; }


        public int UserRole { get; set; }


    }
}
