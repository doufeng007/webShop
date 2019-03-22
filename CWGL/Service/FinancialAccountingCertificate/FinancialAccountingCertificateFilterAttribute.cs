using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CWGL
{

    public class FinancialAccountingCertificateFilterAttribute : ActionFilterAttribute
    {
        private readonly IFinancialAccountingCertificateAppService _financialAccountingCertificateAppService;
        public FinancialAccountingCertificateFilterAttribute(IFinancialAccountingCertificateAppService financialAccountingCertificateAppService)
        {
            _financialAccountingCertificateAppService = financialAccountingCertificateAppService;
        }

        public ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput FACInput { get; set; } = null;


        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            if (this.FACInput != null && this.FACInput.IsSaveFAC)
            {
                ///
                ///_financialAccountingCertificateAppService.CreateOrUpdate(this.FACInput.FACData);
                _financialAccountingCertificateAppService.CreateOrUpdateWithOutNLP(this.FACInput.FACData, this.FACInput.IsUpdateForChange, this.FACInput.FlowId);
            }
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var argument = context.ActionArguments["input"];
            if (argument != null && argument is ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput)
            {
                var argumentmodel = (ICreateOrUpdateFinancialAccountingCertificateFilterAttributeInput)argument;
                this.FACInput = argumentmodel;
            }

        }



    }
}
