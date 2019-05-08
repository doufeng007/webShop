using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Abp.File;

namespace B_H5
{
    /// <summary>
    /// 销售额明显
    /// </summary>
    public class B_AgencySalesListOutputDto
    {

        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid CategroyId { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string CategroyName { get; set; }

        /// <summary>
        /// 销售额
        /// </summary>
        public decimal Sales { get; set; }


        /// <summary>
        /// 销售折扣
        /// </summary>
        public decimal Discount { get; set; }


        /// <summary>
        /// 商品类别缩略图
        /// </summary>
        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();


    }

    /// <summary>
    /// 类别销售额明细
    /// </summary>
    public class B_AgencySalesCategroyListOutputDto
    {

        public Guid AgencyId { get; set; }

        /// <summary>
        /// 代理名称
        /// </summary>
        public string AgencyName { get; set; }

        /// <summary>
        /// 代理级别名称
        /// </summary>
        public Guid AgencyLeavelId { get; set; }

        /// <summary>
        /// 代理级别名称
        /// </summary>
        public string AgencyLeavelName { get; set; }

        /// <summary>
        /// 销售额
        /// </summary>
        public decimal Sales { get; set; }

        /// <summary>
        /// 代理头像
        /// </summary>
        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();
    }
}
