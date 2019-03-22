using Abp.AutoMapper;
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
    /// 建设项目造价咨询完成比例表
    /// </summary>
    [Table("ProjectPersentFinishAllot")]
    public class ProjectPersentFinishAllot : FullAuditedEntity<Guid>
    {
        public Guid ProjectId { get; set; }

        public Guid FinishId { get; set; }


        public Guid AuditMembeId { get; set; }


        public bool IsMain { get; set; }


    }

}
