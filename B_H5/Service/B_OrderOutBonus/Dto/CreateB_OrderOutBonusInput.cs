using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_OrderOutBonus))]
    public class CreateB_OrderOutBonusInput 
    {
        #region 表字段
        /// <summary>
        /// Amout
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Amout { get; set; }

		
        #endregion
    }
}