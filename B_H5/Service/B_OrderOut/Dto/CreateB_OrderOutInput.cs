using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_OrderOut))]
    public class CreateB_OrderOutInput 
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
        /// DeliveryFee
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// PayAmout
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal PayAmout { get; set; }

        /// <summary>
        /// GoodsPayment
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal GoodsPayment { get; set; }

        /// <summary>
        /// Balance
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Balance { get; set; }

        /// <summary>
        /// AddressId
        /// </summary>
        public Guid AddressId { get; set; }

        public string Remark { get; set; }


        public List<CreateB_OrderDetailInput> Goods { get; set; } = new List<CreateB_OrderDetailInput>();



        #endregion
    }
}