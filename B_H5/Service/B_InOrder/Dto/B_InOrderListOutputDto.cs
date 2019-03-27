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
    public class B_InOrderListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string OrderCode { get; set; }


        public int Status { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public string StatusTitle { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime LastTime { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 商品缩略图
        /// </summary>
        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal GoodsPrice { get; set; }


        /// <summary>
        /// 商品数量
        /// </summary>
        public int GoodsNumber { get; set; }

    }
}
