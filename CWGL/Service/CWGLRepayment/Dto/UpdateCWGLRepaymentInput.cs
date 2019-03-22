using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace CWGL
{
    public class UpdateCWGLRepaymentInput : CreateCWGLRepaymentInput, ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput
    {
		
       public Guid InStanceId { get; set; }
       public bool IsUpdateForChange { get; set; }
        public bool IsSaveFAC { get; set; } = false;
        public CreateOrUpdateFinancialAccountingCertificateInput FACData { get; set; } = new CreateOrUpdateFinancialAccountingCertificateInput();
    }
}