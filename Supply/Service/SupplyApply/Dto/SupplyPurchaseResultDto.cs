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
    public class SupplyPurchaseResultDto
    {
        public Guid Id { get; set; }
        public Guid SupplyPurchasePlanId { get; set; }

        public Guid SupplyId { get; set; }

        public string SupplyName { get; set; }

        public string SupplyCode { get; set; }

        public string Version { get; set; }

        public int Type { get; set; }

        public string Type_Name { get; set; }

        public decimal Money { get; set; }

        public string Unit { get; set; }


        //public string UserId { get; set; }

        //public string User_Name { get; set; }


    }

}
