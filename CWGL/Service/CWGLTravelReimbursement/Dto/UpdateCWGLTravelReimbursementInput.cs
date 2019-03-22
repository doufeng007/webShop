using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWGL
{
    public class UpdateCWGLTravelReimbursementInput : CreateCWGLTravelReimbursementInput, ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput
    {
        [Required]
        public Guid Id { get; set; }

        public bool IsUpdateForChange { get; set; }
        public bool IsSaveFAC { get; set; }
        public CreateOrUpdateFinancialAccountingCertificateInput FACData { get; set; } = new CreateOrUpdateFinancialAccountingCertificateInput();
    }
}