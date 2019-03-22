using System;
using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;

namespace HR
{
    [AutoMapTo(typeof(EmployeeAdjustPost))]
    public class CreateEmployeeAdjustPostInput: CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 情况说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 调入部门
        /// </summary>
        public string AdjustDepId { get; set; }

        /// <summary>
        /// 申请职位
        /// </summary>
        public Guid AdjustPostId { get; set; }


        #endregion
    }
}