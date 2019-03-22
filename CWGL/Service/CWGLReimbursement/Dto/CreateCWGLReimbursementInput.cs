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
    [AutoMapTo(typeof(CWGLReimbursement))]
    public class CreateCWGLReimbursementInput : CreateWorkFlowInstance
    {
        #region 表字段

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
        [Required]
        [MaxLength(200)]
        public string Note { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        public int? Nummber { get; set; }

        /// <summary>
        /// 关联备用金
        /// </summary>
        public Guid BorrowMoneyId { get; set; }



		public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
}