using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OABorrow))]
    public class OABorrowInputDto : CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }

        public string Reason { get; set; }

        public decimal? Money { get; set; }

        public int BorrowDays { get; set; }

        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
        public string UserName { get; set; }
        public int? Status { get; set; }
    }

    [AutoMap(typeof(OABorrow))]
    public class OABorrowDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Reason { get; set; }

        public decimal? Money { get; set; }

        public int BorrowDays { get; set; }

        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
        public string UserName { get; set; }
        public int? Status { get; set; }
    }
    [AutoMap(typeof(OABorrow))]
    public class OABorrowListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal? Money { get; set; }

        public int BorrowDays { get; set; }

        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
        public string UserName { get; set; }

    }
}
