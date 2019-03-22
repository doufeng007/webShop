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
    /// oa绩效
    /// </summary>
    [Table("OAPerformance")]
    public class OAPerformance : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }
       
        /// <summary>
        /// 上月计划
        /// </summary>
        public string PlanTask { get; set; }
        /// <summary>
        /// 完成计划
        /// </summary>
        public string FinishTask { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public decimal? FinishPersent { get; set; }
        /// <summary>
        /// 自我评价
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 领导评价
        /// </summary>
        public string LeaderComment { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }
        public int? Status { get; set; }
        /// <summary>
        /// 领导评分
        /// </summary>
        public int? Score { get; set; }

        public Guid? Main_id { get; set; }
    }
}
