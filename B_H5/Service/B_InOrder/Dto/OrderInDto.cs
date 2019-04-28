using Abp.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace B_H5
{
    /// <summary>
    /// 进货dto
    /// </summary>
    public class OrderInDto
    {
        public Guid Id { get; set; }


        public string OrderNo { get; set; }

        public DateTime CreationTime { get; set; }


        public InOrderStatusEnum Status { get; set; }


        /// <summary>
        /// 进货数量
        /// </summary>
        public int Number { get; set; }


        /// <summary>
        /// Amout
        /// </summary>
        public decimal Amout { get; set; }

        /// <summary>
        /// 货款
        /// </summary>
        public decimal GoodsPayment { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }


        public Guid CategroyId { get; set; }

        public string CategroyTitle { get; set; }

        /// <summary>
        /// 进货商品类别缩略图
        /// </summary>
        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();

    }


    
}
