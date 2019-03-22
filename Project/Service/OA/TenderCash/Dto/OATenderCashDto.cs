using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OATenderCash))]
    public class OATenderCashInputDto: CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }
        /// <summary>
        /// 收款单位
        /// </summary>
        public string ToCompany { get; set; }

        public string BankName { get; set; }

        public string Account { get; set; }


        /// <summary>
        /// 保证金金额
        /// </summary>
        public decimal? CashPrice { get; set; }
        /// <summary>
        /// 金额大写
        /// </summary>
        public string CashPriceUp { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Des { get; set; }

        public string Files { get; set; }
        public List<GetAbpFilesOutput> FileList { get; set; }

        public int Status { get; set; }

        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
    }
    [AutoMap(typeof(OATenderCash))]
    public class OATenderCashDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }
        /// <summary>
        /// 收款单位
        /// </summary>
        public string ToCompany { get; set; }
        public List<GetAbpFilesOutput> FileList { get; set; }

        public string BankName { get; set; }

        public string Account { get; set; }


        /// <summary>
        /// 保证金金额
        /// </summary>
        public decimal? CashPrice { get; set; }
        /// <summary>
        /// 金额大写
        /// </summary>
        public string CashPriceUp { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Des { get; set; }

        public string Files { get; set; }

        public int Status { get; set; }

        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
    }
    [AutoMap(typeof(OATenderCash))]
    public class OATenderCashListDto:BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }
        /// <summary>
        /// 收款单位
        /// </summary>
        public string ToCompany { get; set; }
        public string BankName { get; set; }
        /// <summary>
        /// 保证金金额
        /// </summary>
        public decimal? CashPrice { get; set; }
        /// <summary>
        /// 金额大写
        /// </summary>
        public string CashPriceUp { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
