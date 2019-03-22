using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OAPay))]
    public class OAPayInputDto : CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }

        public string ToCompanyName { get; set; }

        public string ProjectName { get; set; }

        public string No { get; set; }

        public decimal? Money { get; set; }

        public string Content { get; set; }
        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
        public string UserName { get; set; }

        public int? Status { get; set; }
    }
    [AutoMap(typeof(OAPay))]
    public class OAPayDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string ToCompanyName { get; set; }

        public string ProjectName { get; set; }

        public string No { get; set; }

        public decimal? Money { get; set; }

        public string Content { get; set; }
        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
        public string UserName { get; set; }

        public int? Status { get; set; }
    }

    [AutoMap(typeof(OAPay))]
    public class OAPayListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string ToCompanyName { get; set; }

        public string ProjectName { get; set; }

        public string No { get; set; }

        public decimal? Money { get; set; }

        public string UserName { get; set; }

    }
}
