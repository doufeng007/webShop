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
    [Table("ProjectPersentFinishResult")]
    public class ProjectPersentFinishResult : FullAuditedEntity<Guid>
    {
        public Guid ProjectId { get; set; }

        public long UserId { get; set; }


        public Guid AllotId { get; set; }


        public int ResultType { get; set; }


        /// <summary>
        /// 
        /// </summary>
      
        public string Files { get; set; }

        
        public string CJZFiles { get; set; }


        public decimal? AuditAmount { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 工程师自己确定的完成比例
        /// </summary>
        public decimal? SurePersent { get; set; }

        /// <summary>
        /// 项目经理确认的完成比例
        /// </summary>
        public decimal? ManagerSurePersent { get; set; }
    }


    
}
