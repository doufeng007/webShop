using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CWGL
{
    [AutoMapTo(typeof(FinancialAccountingCertificate))]
    public class CreateFinancialAccountingCertificateInput
    {
        #region 表字段
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 业务编码
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 业务关联用户
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 记账人员
        /// </summary>
        public long? KeepUserId { get; set; }

        /// <summary>
        /// 审核人员
        /// </summary>
        public long? AuditUserId { get; set; }

        /// <summary>
        /// 出纳人员
        /// </summary>
        public long? CashierUserId { get; set; }

        /// <summary>
        /// 制单人员
        /// </summary>
        public long? MakeUserId { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }


        /// <summary>
        /// 项目名称
        /// </summary>
        [DisplayName(@"项目名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        [DisplayName(@"地区")]
        [MaxLength(50)]
        public string Region { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [DisplayName(@"类别")]
        public Guid? Type { get; set; }

        /// <summary>
        /// 借方金额合计
        /// </summary>
        public decimal TotalDebitAmount { get; set; }

        /// <summary>
        /// 贷方金额合计
        /// </summary>
        public decimal TotalCreditAmount { get; set; }


        public bool IsResultChangeByUser { get; set; } = false;

        public string ResultId { get; set; }

        public List<CreateFACertificateDetailInput> Details { get; set; } = new List<CreateFACertificateDetailInput>();

        public Guid? GroupId { get; set; }

        #endregion
    }
}