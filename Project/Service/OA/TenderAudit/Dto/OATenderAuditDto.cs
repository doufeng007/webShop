using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OATenderAudit))]
    public class OATenderAuditInputDto: CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// 投标金额
        /// </summary>
        public decimal? TenderPrice { get; set; }
        /// <summary>
        /// 建设单位
        /// </summary>
        public string Builder { get; set; }

        public string Content { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }
        public string Files { get; set; }

        public int Status { get; set; }

        public string AuditUser { get; set; }

        public string AuditUserText { get; set; }
    }
    [AutoMap(typeof(OATenderAudit))]
    public class OATenderAuditDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// 投标金额
        /// </summary>
        public decimal? TenderPrice { get; set; }
        /// <summary>
        /// 建设单位
        /// </summary>
        public string Builder { get; set; }

        public string Content { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }
        public string Files { get; set; }

        public int Status { get; set; }

        public string AuditUser { get; set; }

        public string AuditUserText { get; set; }
    }

    [AutoMap(typeof(OATenderAudit))]
    public class OATenderAuditListDto: BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// 投标金额
        /// </summary>
        public decimal? TenderPrice { get; set; }
        /// <summary>
        /// 建设单位
        /// </summary>
        public string Builder { get; set; }
        public string AuditUser { get; set; }

        public string AuditUserText { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
