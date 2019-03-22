using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMap(typeof(EmployeePlan))]
    public class EmployeePlanListDto: BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 面试者
        /// </summary>
        public string ApplyUser { get; set; }
        /// <summary>
        /// 应聘者联系方式
        /// </summary>
        public string Phone { get; set; }
        ///// <summary>
        ///// 应聘部门
        ///// </summary>
        //public long ApplyOrgId { get; set; }
        ///// <summary>
        ///// 应聘部门
        ///// </summary>
        //public string ApplyOrgName { get; set; }
        /// <summary>
        /// 应聘职位
        /// </summary>
        public Guid? ApplyPostId { get; set; }
        ///// <summary>
        ///// 应聘职位
        ///// </summary>
        //public string ApplyPostName { get; set; }
        /// <summary>
        /// 应聘职位
        /// </summary>
        public string ApplyJob { get; set; }
        /// <summary>
        /// 面试编号
        /// </summary>
        public string ApplyNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();

    }
}
