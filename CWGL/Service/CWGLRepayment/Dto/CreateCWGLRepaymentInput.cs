using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using CWGL.Enums;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLRepayment))]
    public class CreateCWGLRepaymentInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [MaxLength(500,ErrorMessage = "流程查阅人员长度必须小于500")]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Status { get; set; }

        /// <summary>
        /// 借款编号
        /// </summary>
        public Guid BorrowMoneyId { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public MoneyMode Mode { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [MaxLength(100,ErrorMessage = "银行名称长度必须小于100")]
        public string BankName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        [MaxLength(64,ErrorMessage = "卡号长度必须小于64")]
        public string CardNumber { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        [MaxLength(100,ErrorMessage = "开户行名称长度必须小于100")]
        public string BankOpenName { get; set; }


        #endregion
    }
}