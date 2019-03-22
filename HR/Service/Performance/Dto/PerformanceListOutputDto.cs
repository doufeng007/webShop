using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace HR
{
    [AutoMapFrom(typeof(Performance))]
    public class PerformanceListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 事项
        /// </summary>
        public string Matter { get; set; }

        /// <summary>
        /// 记录
        /// </summary>
        public string Record { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public PerformanceType Type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
    public class PerformanceDataListOutputDto {
        public string Title { get; set; }
        public string Name { get; set; }
        public List<PerformanceListOutputDto> Nodes { get; set; } = new List<PerformanceListOutputDto>();
    }
    public class PerformanceDataOutputDto
    {
        public int WorkNumber { get; set; }
        public string Post { get; set; }
        public string Org { get; set; }
        public string Name { get; set; }
        public string ChargeLeader { get; set; }
        public int NoDataScore { get; set; }
        public int DataScore { get; set; }
        public int Score { get; set; }
        public long UserId { get; set; }
    }
}
