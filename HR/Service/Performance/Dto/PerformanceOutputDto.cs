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
    [AutoMapFrom(typeof(Performance))]
    public class PerformanceOutputDto 
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
}
