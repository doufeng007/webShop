using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using Supply.Entity;
using System;
using System.Collections.Generic;
using System.Text;
namespace Supply
{
    public class SupplyPurchaseMainDto : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public DateTime CreationTime { get; set; }


        public List<SupplyPurchasePlanDto> Plans { get; set; }

        public SupplyPurchaseMainDto()
        {
            this.Plans = new List<SupplyPurchasePlanDto>();
        }
    }

}
