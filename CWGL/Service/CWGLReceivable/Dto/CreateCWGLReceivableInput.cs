using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using CWGL.Enums;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLReceivable))]
    public class CreateCWGLReceivableInput : CreateWorkFlowInstance, ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput
    {
        #region 表字段
        /// <summary>
        /// 收款人
        /// </summary>
        [MaxLength(20,ErrorMessage = "收款人长度必须小于20")]
        [Required(ErrorMessage = "必须填写收款人")]
        public string UserName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [MaxLength(50,ErrorMessage = "客户名称长度必须小于50")]
        [Required(ErrorMessage = "必须填写客户名称")]
        public string Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal Money { get; set; }

        /// <summary>
        /// 付款方式
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

        /// <summary>
        /// 事由摘要
        /// </summary>
        [Required(ErrorMessage = "必须填写事由摘要")]
        public string Note { get; set; }

        /// <summary>
        /// 纸质凭证
        /// </summary>
        public int? Nummber { get; set; }

        public bool IsSaveFAC { get; set; } = false;
        public CreateOrUpdateFinancialAccountingCertificateInput FACData { get; set; } = new CreateOrUpdateFinancialAccountingCertificateInput();


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();


        public bool IsUpdateForChange { get; set; }
        #endregion
    }
}