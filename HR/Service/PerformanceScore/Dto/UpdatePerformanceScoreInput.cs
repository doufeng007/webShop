using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace HR
{
    public class UpdatePerformanceScoreInput 
    { 
        public Guid Id { get; set; }
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
    }
}