using System;
using System.Collections.Generic;
using System.Text;

namespace B_H5.Service.CloudService.Dto
{
    /// <summary>
    /// 出入库单确认接口参数（必填项）
    /// </summary>
    public class PurchaseOutinorderConfirmInput
    {
        public string actionType { get; set; }


        public string wareHouseCode { get; set; }


        public string syncId { get; set; }


        public string barCode { get; set; }


        public string inventoryType { get; set; }


        public int planQuantity { get; set; }

        public int quantity { get; set; }


    }
}
