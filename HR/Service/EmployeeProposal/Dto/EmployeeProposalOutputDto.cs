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

namespace HR
{
    [AutoMapFrom(typeof(EmployeeProposal))]
    public class EmployeeProposalOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string WorkNumber { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public string PostName { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }
        public string OrgName { get; set; }
        public string IssueUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        public ProposalType Type { get; set; }

        public string ParticipateUser_Name { get; set; }

        /// <summary>
        /// 回复
        /// </summary>
        public string Comment { get; set; }



        public bool IsIssue { get; set; }
        public long? OrgId { get; set; }
        public long? IssueUserId { get; set; }
        public Guid? SingleProjectId { get; set; }
        public int? IssueType { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();


    }
}
