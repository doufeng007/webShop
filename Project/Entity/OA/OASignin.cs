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
    /// oa签到
    /// </summary>
    [Table("OASignin")]
    public class OASignin : FullAuditedEntity<Guid>
    {
    }
}
