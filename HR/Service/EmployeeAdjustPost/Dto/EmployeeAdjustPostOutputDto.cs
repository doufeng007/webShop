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
    public class EmployeeAdjustPostOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 情况说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 调入部门
        /// </summary>
        public long AdjustDepId { get; set; }

        public string AdjustDepName { get; set; }

        /// <summary>
        /// 申请职位
        /// </summary>
        public Guid AdjustPostId { get; set; }

        public Guid PostId { get; set; }

        public string AdjustPostName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// DealWithUsers
        /// </summary>
        public string DealWithUsers { get; set; }

        public string WorkNumber { get; set; }
        public string UserName { get; set; }
        public string DepartmentName { get; set; }
        public string PostName { get; set; }
    }
}