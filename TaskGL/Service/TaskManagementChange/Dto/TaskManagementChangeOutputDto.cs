using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;
using TaskGL.Enum;

namespace TaskGL
{
    [AutoMapFrom(typeof(TaskManagementChange))]
    public class TaskManagementChangeOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 变更类型
        /// </summary>
        public TaskManagementChangeTypeEnum Type { get; set; }

        /// <summary>
        /// 变更编号
        /// </summary>
        public Guid TaskManagementId { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public TaskManagementReasonEnum Reason { get; set; }

        /// <summary>
        /// 评估
        /// </summary>
        public string Assessment { get; set; }

        /// <summary>
        /// 要求
        /// </summary>
        public string Requirement { get; set; }


        /// <summary>
        /// 绩效分
        /// </summary>
        public int? PerformanceScore { get; set; }

        /// <summary>
        /// 精神分
        /// </summary>
        public int? SpiritScore { get; set; }
        public long CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }


    }
}
