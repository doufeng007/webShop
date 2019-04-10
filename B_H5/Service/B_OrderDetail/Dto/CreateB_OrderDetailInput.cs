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
        /// BId
        /// </summary>
        public Guid BId { get; set; }

        /// <summary>
        /// BType
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int BType { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Number { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid CategroyId { get; set; }

        /// <summary>
        /// GoodsId
        /// </summary>
        public Guid? GoodsId { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        public decimal? Amout { get; set; }


		
        #endregion
    }
}