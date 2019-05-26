using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_OrderDetail))]
    public class CreateB_OrderDetailInput
    {
        #region 表字段

        /// <summary>
        /// 数量
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "")]
        public int Number { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public Guid GoodsId { get; set; }

        /// <summary>
        /// 商品的一级类别
        /// </summary>
        public Guid CategroyId { get; set; }


        public decimal Amout { get; set; }


        #endregion
    }
}