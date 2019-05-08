using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_AgencySales))]
    public class CreateB_AgencySalesInput 
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// AgencyId
        /// </summary>
        public Guid AgencyId { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid CategroyId { get; set; }

        /// <summary>
        /// Sales
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Sales { get; set; }

        /// <summary>
        /// SalesDate
        /// </summary>
        public DateTime SalesDate { get; set; }

        /// <summary>
        /// Profit
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Profit { get; set; }

        /// <summary>
        /// Discount
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Discount { get; set; }


		
        #endregion
    }
}