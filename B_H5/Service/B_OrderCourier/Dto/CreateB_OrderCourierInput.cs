using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_OrderCourier))]
    public class CreateB_OrderCourierInput 
    {
        #region 表字段
        /// <summary>
        /// OrderId
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// CourierNum
        /// </summary>
        [MaxLength(100,ErrorMessage = "CourierNum长度必须小于100")]
        [Required(ErrorMessage = "必须填写CourierNum")]
        public string CourierNum { get; set; }

        /// <summary>
        /// CourierName
        /// </summary>
        [MaxLength(50,ErrorMessage = "CourierName长度必须小于50")]
        [Required(ErrorMessage = "必须填写CourierName")]
        public string CourierName { get; set; }

        /// <summary>
        /// DeliveryFee
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Status { get; set; }


		
        #endregion
    }
}