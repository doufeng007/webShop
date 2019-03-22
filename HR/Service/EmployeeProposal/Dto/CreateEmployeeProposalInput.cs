using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;

namespace HR
{
    [AutoMapTo(typeof(EmployeeProposal))]
    public class CreateEmployeeProposalInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ProposalType Type { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string ParticipateUser { get; set; }
        public bool IsIssue { get; set; } = false;
        public long? OrgId { get; set; }
        public long? IssueUserId { get; set; }
        public Guid? SingleProjectId { get; set; }
        public int? IssueType { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
}