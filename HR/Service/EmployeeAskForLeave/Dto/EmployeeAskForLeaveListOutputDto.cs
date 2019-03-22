using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    public class EmployeeAskForLeaveListOutputDto : BusinessWorkFlowListOutput
    {

        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// BeginTime
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; }

        

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// TenantId
        /// </summary>
        public int? TenantId { get; set; }

        public decimal? Hours { get; set; }

        public long? RelationUserId { get; set; }


        public long OrgId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public string PostName { get; set; }

        public string UserId_Name { get; set; }
    }
}
