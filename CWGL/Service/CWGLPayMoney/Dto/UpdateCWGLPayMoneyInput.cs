using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace CWGL
{
    public class UpdateCWGLPayMoneyInput : CreateCWGLPayMoneyInput, ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput
    {
        public Guid Id { get; set; }
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }
        public string FlowNumber { get; set; }
        public CreateOrUpdateFinancialAccountingCertificateInput FACData { get; set; } = new CreateOrUpdateFinancialAccountingCertificateInput();
        public bool IsSaveFAC { get; set; }
    }
}