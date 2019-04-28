using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_InventoryAddRecord))]
    public class CreateB_InventoryAddRecordInput 
    {
        #region 表字段
        /// <summary>
        /// 商品id
        /// </summary>
        public Guid Goodsid { get; set; }

        /// <summary>
        /// 新增库存数量
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Count { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public B_InventoryAddConfigEnum Status { get; set; }

        /// <summary>
        /// ConfirmUserId
        /// </summary>
        public long? ConfirmUserId { get; set; }


		
        #endregion
    }
}