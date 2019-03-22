using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OAPettyCash))]
    public class OAPettyCashInputDto : CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public string SignUper { get; set; }
        public string Title { get; set; }
        public decimal? Money { get; set; }

        public string Reason { get; set; }
        public string AuditUser { get; set; }
        public string UserName { get; set; }
        public string AuditUserText { get; set; }
        public int? Status { get; set; }
    }

    [AutoMap(typeof(OAPettyCash))]
    public class OAPettyCashDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SignUper { get; set; }

        public decimal? Money { get; set; }

        public string Reason { get; set; }
        public string AuditUser { get; set; }
        public string UserName { get; set; }
        public string AuditUserText { get; set; }
        public int? Status { get; set; }
    }

    [AutoMap(typeof(OAPettyCash))]
    public class OAPettyCashListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SignUper { get; set; }

        public decimal? Money { get; set; }
        public string AuditUser { get; set; }
        public string UserName { get; set; }
        public string AuditUserText { get; set; }
    }
}
