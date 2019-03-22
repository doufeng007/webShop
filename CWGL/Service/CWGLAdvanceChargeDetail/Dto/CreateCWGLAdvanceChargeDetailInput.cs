using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Extensions;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLAdvanceChargeDetail))]
    public class CreateCWGLAdvanceChargeDetailInput 
    {
        #region 表字段
        /// <summary>
        /// 预付款编号
        /// </summary>
        [Required]
        public Guid AdvanceChargeId { get; set; }

        /// <summary>
        /// 预付金额
        /// </summary>
        [Required]
        [Money]
        public decimal Money { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        [Required]
        public int Mode { get; set; }

        /// <summary>
        /// 付款银行
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        public string BankOpenName { get; set; }


		
        #endregion
    }
}