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

namespace B_H5
{
    [AutoMapFrom(typeof(B_AgencySales))]
    public class B_AgencySalesOutputDto 
    {
        
        /// <summary>
        /// 总销售额
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// 利润
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Bonus { get; set; }

        /// <summary>
        /// 销售折扣
        /// </summary>
        public decimal Discount { get; set; }


    }
}
