using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace  ZCYX.FRMSCore.Application
{
    [AutoMapFrom(typeof(TaskManagementRelation))]
    public class TaskManagementRelationListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 流程编号
        /// </summary>
        public Guid FlowId { get; set; }

        /// <summary>
        /// 关联编号
        /// </summary>
        public Guid InStanceId { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        public Guid TaskManagementId { get; set; }


    }
}
