using Abp.Application.Services.Dto;
using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    public class MeetingIssueByEvent: EventData
    {
        /// <summary>
        /// 事项
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 汇报部门
        /// </summary>
        public long? OrgId { get; set; }

        /// <summary>
        /// 汇报人
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


        public int IssueType { get; set; }


        public Guid? SingleProjectId { get; set; }
        public Guid? RelationProposalId { get; set; }
    }
}
