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
    [AutoMapFrom(typeof(PerformanceAppeal))]
    public class PerformanceAppealListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        public int DataCount { get; set; }
        public int NoDataCount { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public List<WorkTempPost> Post { get; set; } = new List<WorkTempPost>();


    }
}
