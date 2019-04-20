using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_Withdrawal))]
    public class CreateB_WithdrawalInput 
    {
        #region 表字段
        /// <summary>
        /// 银行名称
        /// </summary>
        [MaxLength(100,ErrorMessage = "BankName长度必须小于100")]
        [Required(ErrorMessage = "必须填写BankName")]
        public string BankName { get; set; }

        /// <summary>
        /// 支行名称
        /// </summary>
        [MaxLength(200,ErrorMessage = "BankBranchName长度必须小于200")]
        [Required(ErrorMessage = "必须填写BankBranchName")]
        public string BankBranchName { get; set; }

        /// <summary>
        /// 开号姓名
        /// </summary>
        [MaxLength(50,ErrorMessage = "BankUserName长度必须小于50")]
        [Required(ErrorMessage = "必须填写BankUserName")]
        public string BankUserName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        [MaxLength(50,ErrorMessage = "BankNumber长度必须小于50")]
        [Required(ErrorMessage = "必须填写BankNumber")]
        public string BankNumber { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Amout { get; set; }

        ///// <summary>
        ///// Reason
        ///// </summary>
        //[MaxLength(500,ErrorMessage = "Reason长度必须小于500")]
        //public string Reason { get; set; }

        ///// <summary>
        ///// Remark
        ///// </summary>
        //[MaxLength(500,ErrorMessage = "Remark长度必须小于500")]
        //public string Remark { get; set; }


		
        #endregion
    }
}