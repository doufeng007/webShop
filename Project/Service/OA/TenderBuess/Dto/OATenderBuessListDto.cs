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
    [AutoMapFrom(typeof(OATenderBuess))]
    public class OATenderBuessListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public DateTime ApplyDate { get; set; }

        public string ProjectCode { get; set; }

        public string ProjectName { get; set; }

        public decimal? CashPrice { get; set; }


        public DateTime CreationTime { get; set; }

    }
}
