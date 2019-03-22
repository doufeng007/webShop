using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;

namespace CWGL
{
    public class UpdateCWGLReimbursementInput : CreateCWGLReimbursementInput, ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput
    {
        public Guid Id { get; set; }
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }

        public CreateOrUpdateFinancialAccountingCertificateInput FACData { get; set; } = new CreateOrUpdateFinancialAccountingCertificateInput();
        public bool IsSaveFAC { get; set; }
    }
}