using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    /// <summary>
    /// 获取待沟通、面试中列表
    /// </summary>
    public  class EmployeePlanSearchInput: WorkFlowPagedAndSortedInputDto
    {
        /// <summary>
        /// 0：待沟通 1：面试中
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string SearchKey { get; set; }
    }
}
