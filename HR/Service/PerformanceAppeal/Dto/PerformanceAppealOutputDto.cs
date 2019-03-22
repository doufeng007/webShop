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

namespace HR
{
    [AutoMapFrom(typeof(PerformanceAppeal))]
    public class PerformanceAppealOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        public List<PerformanceAppealDetailOutputDto> Details { get; set; } = new List<PerformanceAppealDetailOutputDto>();
    }
    public class PerformanceAppealDetailOutputDto 
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public PerformanceType Type { get; set; }
        public Guid PerformanceId { get; set; }
        public int? AfterScore { get; set; }
        public PerformanceListOutputDto  PerformanceInfo { get; set; }
        public string Content { get; set; }
    }
}
