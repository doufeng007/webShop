using System;
using System.Collections.Generic;
using System.Text;

namespace B_H5
{
    /// <summary>
    /// 进货input
    /// </summary>
    public class OrderInInput
    {
        /// <summary>
        /// 商品类别id
        /// </summary>
        public Guid CategroyId { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 货款
        /// </summary>
        public decimal GoodsPayment { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }
    }


    /// <summary>
    /// 用户当前余额、货款数据
    /// </summary>
    public class UserBlanceDto
    {
        /// <summary>
        /// 可用贷款
        /// </summary>
        public decimal UserGoodsPayment { get; set; }

        /// <summary>
        /// 可用余额
        /// </summary>
        public decimal UserBalance { get; set; }

    }
}
