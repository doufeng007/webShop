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
    [Table("ProjectPersentFinish")]
    public class ProjectPersentFinish : FullAuditedEntity<Guid>
    {

        public Guid ProjectId { get; set; }
        /// <summary>
        /// 工程类别
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// 完成项目：1.熟悉图纸、建模及算量、清单及算价、汇总核算
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 完成度百分比
        /// </summary>
        public decimal Persent { get; set; }

        /// <summary>
        /// 送审金额上限
        /// </summary>
        public decimal MoneyLimitMax { get; set; }
        /// <summary>
        /// 送审金额下限
        /// </summary>
        public decimal MoneyLimitMin { get; set; }


        public int WorkDay { get; set; }
    }
    [AutoMap(typeof(ProjectPersentFinish))]
    public class ProjectPersentFinishDto
    {

        public Guid Id { get; set; }
        /// <summary>
        /// 工程类别
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// 完成项目：1.熟悉图纸、建模及算量、清单及算价、汇总核算
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 完成度百分比
        /// </summary>
        public decimal Persent { get; set; }

        /// <summary>
        /// 送审金额上限
        /// </summary>
        public decimal MoneyLimitMax { get; set; }
        /// <summary>
        /// 送审金额下限
        /// </summary>
        public decimal MoneyLimitMin { get; set; }

        public int WorkDay { get; set; }
    }
}
