using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace MeetingGL
{
    [AutoMapFrom(typeof(MeetingIssue))]
    public class MeetingIssueListOutputDto 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 事项
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 汇报部门
        /// </summary>
        public long? OrgId { get; set; }


        /// <summary>
        /// 汇报部门名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 汇报人
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 汇报人名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public MeetingIssueStatus Status { get; set; }

                        
        public string StatusTitle { get; set; }

        /// <summary>
        /// 议题类型
        /// </summary>
        public IssueType IssueType { get; set; }

        /// <summary>
        /// 关联项目id
        /// </summary>
        public Guid? SingleProjectId { get; set; }


        /// <summary>
        /// 关联项目名称
        /// </summary>
        public string SingleProjecetName { get; set; }


    }
}
