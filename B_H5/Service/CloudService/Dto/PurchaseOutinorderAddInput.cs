using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace B_H5.Service.CloudService.Dto
{
    public class PurchaseOutinorderAddInput
    {
        public string actionType { get; set; }

        public string wareHouseCode { get; set; }


        public string barCode { get; set; }

        /// <summary>
        /// 库存类型：NORMAL-正常（可售） SCRAP-次品（不可售）
        /// </summary>
        public string inventoryType { get; set; }


        /// <summary>
        /// 商品数量
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "商品数量需大于0")]
        public int quantity { get; set; }

    }
}
