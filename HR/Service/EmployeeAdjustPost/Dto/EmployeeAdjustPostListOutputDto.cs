using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace HR
{
    [AutoMapFrom(typeof(EmployeeAdjustPost))]
    public class EmployeeAdjustPostListOutputDto : BusinessWorkFlowListOutput
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
        /// 情况说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 调入部门
        /// </summary>
        public virtual long AdjustDepId { get; set; }
        public string AdjustDepName { get; set; }

        /// <summary>
        /// 申请职位
        /// </summary>
        public virtual Guid AdjustPostId { get; set; }
        public string AdjustPostName { get; set; }
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// DealWithUsers
        /// </summary>
        public string DealWithUsers { get; set; }


    }
}
