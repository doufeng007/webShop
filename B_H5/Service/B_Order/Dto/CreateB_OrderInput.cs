using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_Order))]
    public class CreateB_OrderInput 
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Amout { get; set; }

        /// <summary>
        /// Stauts
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Stauts { get; set; }


        public OrderAmoutEnum InOrOut { get; set; }

        public Guid BusinessId { get; set; }

        public OrderAmoutBusinessTypeEnum BusinessType { get; set; }

        public string OrderNo { get; set; }

        /// <summary>
        /// 是余额
        /// </summary>
        public bool IsBlance { get; set; }

        /// <summary>
        /// 是货款
        /// </summary>
        public bool IsGoodsPayment { get; set; }

        #endregion
    }
}