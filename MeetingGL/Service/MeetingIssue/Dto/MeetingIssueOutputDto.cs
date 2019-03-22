using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;

namespace MeetingGL
{
    [AutoMapFrom(typeof(MeetingIssue))]
    public class MeetingIssueOutputDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        public Guid IssueId { get; set; }

        /// <summary>
        /// 事项
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 汇报部门
        /// </summary>
        public long? OrgId { get; set; }


        public string OrgName { get; set; }

        /// <summary>
        /// 汇报人
        /// </summary>
        public string UserId { get; set; }


        public string UserName { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        public MeetingIssueStatus Stauts { get; set; }

        public MeetingIssueResultStatus ResultStatus { get; set; }


        public string StautsTitle { get; set; }


        public IssueType IssueType { get; set; }


        public string IssueTypeName { get; set; }

        /// <summary>
        /// 关联项目id
        /// </summary>
        public Guid? SingleProjectId { get; set; }


        /// <summary>
        /// 关联项目名称
        /// </summary>
        public string SingleProjecetName { get; set; }

        /// <summary>
        /// 是否已议
        /// </summary>
        public bool HasPass { get; set; }

        /// <summary>
        /// 准备状态
        /// </summary>
        public string ReadStatus { get; set; }


    }
}
