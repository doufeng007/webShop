using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Extensions;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLPrePaymentDetail))]
    public class CreateCWGLPrePaymentDetailInput 
    {
        #region 表字段
        /// <summary>
        /// 预收款编号
        /// </summary>
        [Required]
        public Guid PrePaymentId { get; set; }

        /// <summary>
        /// 预收金额
        /// </summary>
        [Required]
        [Money]
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        [Required]
        public int Mode { get; set; }

        /// <summary>
        /// 收款银行
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