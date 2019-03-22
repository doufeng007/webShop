using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{

    [AutoMap(typeof(OAFee))]
    public class OAFeeInputDto : CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }

        public string Contet { get; set; }

        public decimal? Money { get; set; }

        public DateTime? FeeDate { get; set; }
        public string AuditUser { get; set; }
        public string UserName { get; set; }
        public string AuditUserText { get; set; }
        public int? Status { get; set; }
    }
    [AutoMap(typeof(OAFee))]
    public class OAFeeDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Contet { get; set; }

        public decimal? Money { get; set; }

        public DateTime? FeeDate { get; set; }
        public string AuditUser { get; set; }
        public string UserName { get; set; }
        public string AuditUserText { get; set; }
        public int? Status { get; set; }
    }

    [AutoMap(typeof(OAFee))]
    public class OAFeeListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public string Title { get; set; }


        public decimal? Money { get; set; }

        public DateTime? FeeDate { get; set; }
        public string AuditUser { get; set; }
        public string UserName { get; set; }
        public string AuditUserText { get; set; }
        
    }
}
