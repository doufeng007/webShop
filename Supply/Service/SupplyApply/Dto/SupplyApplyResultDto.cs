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
    [AutoMap(typeof(SupplyApplySub))]
    public class SupplyApplyResultDto
    {
        public Guid Id { get; set; }
        public Guid ApplyMainId { get; set; }

        public Guid ApplySubId { get; set; }

        public Guid SupplyId { get; set; }

        public string SupplyName { get; set; }

        public string SupplyCode { get; set; }


        public string SupplyVersion { get; set; }

        public decimal SupplyMoney { get; set; }


        


    }

}
