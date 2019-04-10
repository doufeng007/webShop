using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_Goods))]
    public class CreateB_GoodsInput 
    {
        #region 表字段
        /// <summary>
        /// Name
        /// </summary>
        [MaxLength(500,ErrorMessage = "Name长度必须小于500")]
        [Required(ErrorMessage = "必须填写Name")]
        public string Name { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid? CategroyId { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Price { get; set; }

        /// <summary>
        /// Pirce1
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Pirce1 { get; set; }

        /// <summary>
        /// Price2
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Price2 { get; set; }


		
        #endregion
    }
}