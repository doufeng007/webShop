using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace CWGL
{
    public class UpdateCWGLoanInput : CreateCWGLoanInput, ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput
    {
        public Guid Id { get; set; }
        public Guid FlowId { get; set; }
        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        public bool IsUpdateForChange { get; set; }
        public CreateOrUpdateFinancialAccountingCertificateInput FACData { get; set; } = new CreateOrUpdateFinancialAccountingCertificateInput();
        public bool IsSaveFAC { get; set; }
    }
}