using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace CWGL
{
    public class UpdateFinancialAccountingCertificateInput : CreateFinancialAccountingCertificateInput
    {
        public Guid Id { get; set; }


    }


    public class CreateOrUpdateFinancialAccountingCertificateInput : UpdateFinancialAccountingCertificateInput
    {
        public new Guid? Id { get; set; }



      
    }


    public class CreateOrUpdateFinancialAccountingCertificateFilterAttributeInput
    {
        public bool IsSaveFAC { get; set; } = false;

        public CreateOrUpdateFinancialAccountingCertificateInput FACData { get; set; } = new CreateOrUpdateFinancialAccountingCertificateInput();
    }


    public interface ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput
    {
        bool IsSaveFAC { get; set; }


        bool IsUpdateForChange { get; set; }


        Guid FlowId { get; set; }

        CreateOrUpdateFinancialAccountingCertificateInput FACData { get; set; }
    }
}