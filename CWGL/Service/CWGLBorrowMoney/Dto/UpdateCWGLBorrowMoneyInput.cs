using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using CWGL.Enums;
using System.ComponentModel.DataAnnotations;

namespace CWGL
{
    public class UpdateCWGLBorrowMoneyInput : CreateCWGLBorrowMoneyInput, ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput
    {
        public Guid Id { get; set; }
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }
        public DateTime? RepaymentTime { get; set; }
        public bool IsSaveFAC { get; set; } = false;
        public CreateOrUpdateFinancialAccountingCertificateInput FACData { get; set; } = new CreateOrUpdateFinancialAccountingCertificateInput();
        public DateTime? RepayTime { get; set; }
        public MoneyMode? RepayMode { get; set; }
        [MaxLength(100)]
        public string RepayBankName { get; set; }
        [MaxLength(64)]
        public string RepayCardNumber { get; set; }
        [MaxLength(100)]
        public string RepayBankOpenName { get; set; }
    }
}