using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace HR
{
    /// <summary>
    /// 员工离职申请列表
    /// </summary>
    public class EmployeeResignListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 员工工号
        /// </summary>
        public string WorkNumber { get; set; }

        /// <summary>
        /// 离职类别
        /// </summary>
        public EmployeeResignType Type { get; set; }
        public string Type_Name { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        //public EmployeeResignStatus Status { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        public long OrgId { get; set; }
        public string PostIds { get; set; }
        public long? CreatorUserId { get; set; }

        public string DepartmentName { get; set; }

        public string PostName { get; set; }
    }
}
