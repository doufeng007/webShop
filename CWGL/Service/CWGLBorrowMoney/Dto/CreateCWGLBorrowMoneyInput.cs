using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using CWGL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Extensions;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLBorrowMoney))]
    public class CreateCWGLBorrowMoneyInput : CreateWorkFlowInstance
    {
        #region 表字段

        /// <summary>
        /// 借款类型
        /// </summary>
        [Required]
        public BorrowMoney TypeId { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Required]
        [Money]
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>      
        [Required]
        public MoneyMode Mode { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [MaxLength(20)]
        public string BankName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        [MaxLength(30)]
        public string BankOpenName { get; set; }

        /// <summary>
        /// 事由摘要
        /// </summary>
        [MaxLength(200)]
        [Required]
        public string Note { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        public int? Nummber { get; set; }

        public DateTime? RepaymentTime { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
}