using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    [AutoMapFrom(typeof(WorkRecord))]
    public class WorkRecordListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Head { get; set; }

        public string Function { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        public Guid BusinessId { get; set; }

        public int BusinessType { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Remuneration
        /// </summary>
        public decimal Remuneration { get; set; }



        /// <summary>
        /// 数字化绩效
        /// </summary>
        public decimal? DataPerformance { get; set; }
        /// <summary>
        /// 非数字化绩效
        /// </summary>
        public decimal? NoDataPerformance { get; set; }
        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
