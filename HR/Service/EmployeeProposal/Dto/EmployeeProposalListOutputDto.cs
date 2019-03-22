using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Microsoft.AspNetCore.Mvc.Internal;

namespace HR
{
    [AutoMapFrom(typeof(EmployeeProposal))]
    public class EmployeeProposalListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

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
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        public string ParticipateUser { get; set; }
         
        public virtual IEnumerable<DepartmentInfo> DepartmentInfos { get; set; }
             

    }

    public class DepartmentInfo
    {
        public string WorkNumber { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
/// <summary>
/// 部门
/// </summary>
public string DepartmentName { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public string PostName { get; set; }
    }
}
