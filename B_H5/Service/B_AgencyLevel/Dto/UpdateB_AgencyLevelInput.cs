using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace B_H5
{
    public class UpdateB_AgencyLevelInput : CreateB_AgencyLevelInput
    {
		public Guid Id { get; set; }


        /// <summary>
        /// 首充金额
        /// </summary>
        public decimal FirstRechargeAmout { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal Deposit { get; set; }


        /// <summary>
        /// 推荐奖
        /// </summary>
        public decimal RecommendAmout { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
    }
}