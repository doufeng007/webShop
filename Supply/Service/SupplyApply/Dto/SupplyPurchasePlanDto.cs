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
    public class SupplyPurchasePlanDto
    {
        public Guid Id { get; set; }

        public Guid? SupplyApplyMainId { get; set; }

        public Guid? SupplyApplySubId { get; set; }


        public Guid? SupplyPurchaseId { get; set; }

        public string SupplyPurchaseCode { get; set; }


        public string Name { get; set; }

        public string Version { get; set; }

        public int Number { get; set; }

        public string Unit { get; set; }

        public string Money { get; set; }

        public string Des { get; set; }

        public DateTime GetTime { get; set; }

        public string UserId { get; set; }

        public string User_Name { get; set; }

        public int Type { get; set; }


        public string TypeName { get; set; }


        public int DoPurchaseStatus { get; set; }


        public string DoPurchaseStatusTitle { get; set; }

        public int Status { get; set; }

        public DateTime CreationTime { get; set; }

        public List<SupplyPurchaseResultDto> Result { get; set; }

        public SupplyPurchasePlanDto()
        {
            this.Result = new List<SupplyPurchaseResultDto>();
        }

    }

}
