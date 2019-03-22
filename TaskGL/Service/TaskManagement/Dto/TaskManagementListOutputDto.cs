using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using TaskGL.Enum;
using ZCYX.FRMSCore.Application;

namespace TaskGL
{
    /// <summary>
    /// 任务列表
    /// </summary>
    [AutoMapFrom(typeof(TaskManagement))]
    public class TaskManagementListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskManagementTypeEnum? Type { get; set; }


        public string TypeName { get; set; }


        /// <summary>
        /// 任务编号
        /// </summary>
        public string Number { get; set; }



        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }


        /// <summary>
        /// 任务发起人
        /// </summary>
        public long TaskCreateUserId { get; set; }

        /// <summary>
        /// 任务发起人
        /// </summary>
        public string TaskCreateUserName { get; set; }


        /// <summary>
        /// 负责人
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 督办人
        /// </summary>
        public long? Superintendent { get; set; }


        /// <summary>
        /// 督办人
        /// </summary>
        public string SuperintendentName { get; set; }



        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }



        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskManagementStateEnum TaskStatus { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskStatusTitle { get; set; }


        /// <summary>
        /// 是否紧急
        /// </summary>
        public bool? IsUrgent { get; set; }


        /// <summary>
        /// 是否关注
        /// </summary>
        public bool IsFollow { get; set; }


    }
}
