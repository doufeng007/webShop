using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_TeamSaleBonusDetail))]
    public class CreateB_TeamSaleBonusDetailInput
    {
        #region 表字段


        /// <summary>
        /// MaxSale
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "")]
        public decimal MaxSale { get; set; }

        /// <summary>
        /// MinSale
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "")]
        public decimal MinSale { get; set; }

        /// <summary>
        /// Scale
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "")]
        public decimal Scale { get; set; }



        #endregion
    }
}