using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMapFrom(typeof(OAContractCollectionFee))]
    public class OAContractCollectionFeeListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ProjectName { get; set; }

        public long WriteUser { get; set; }

        public string WriteUserName { get; set; }

        public DateTime WriteData { get; set; }

        public string ContractName { get; set; }


        public string UnitA { get; set; }

        public decimal CollectionAmount { get; set; }

        public DateTime CreationTime { get; set; }


    }
}
