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
    [AutoMapFrom(typeof(PerformanceScore))]
    public class PerformanceScoreOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 规则
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 小于等于
        /// </summary>
        public int Than1 { get; set; }

        /// <summary>
        /// 区间大于
        /// </summary>
        public int Than21 { get; set; }

        /// <summary>
        /// 区间小于等于
        /// </summary>
        public int Than22 { get; set; }

        /// <summary>
        /// 大于
        /// </summary>
        public int Than3 { get; set; }

        /// <summary>
        /// 小于等于分数
        /// </summary>
        public int Than1Score { get; set; }

        /// <summary>
        /// 区间分数
        /// </summary>
        public int Than2Score { get; set; }

        /// <summary>
        /// 大于分数
        /// </summary>
        public int Than3Score { get; set; }

        /// <summary>
        /// 分数类型
        /// </summary>
        public int ScoreTypeId { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public int Unit { get; set; }

        /// <summary>
        /// 加分减分选择
        /// </summary>
        public int ScoreType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


		
    }
}
