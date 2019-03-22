using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;
using TaskGL.Enum;

namespace TaskGL
{
    [AutoMapFrom(typeof(TaskManagement))]
    public class TaskManagementOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// 任务要求
        /// </summary>
        public string Requirement { get; set; }


        /// <summary>
        /// 任务发起人
        /// </summary>
        public long TaskCreateUserId { get; set; }

        /// <summary>
        /// 任务发起人
        /// </summary>
        public string TaskCreateUserName { get; set; }



        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskManagementTypeEnum? Type { get; set; }


        public string TypeName { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

       

        

        /// <summary>
        /// 签名文件
        /// </summary>
        public Guid? SignFileId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public long? UserId { get; set; }


        public string UserName { get; set; }

        

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        public bool? IsUrgent { get; set; }

        /// <summary>
        /// 督办人
        /// </summary>
        public long? Superintendent { get; set; }

        /// <summary>
        /// 督办人
        /// </summary>
        public string SuperintendentName { get; set; }

        /// <summary>
        /// 任务说明
        /// </summary>
        public string Explain { get; set; }

        /// <summary>
        /// 绩效量分
        /// </summary>
        public int? PerformanceScore { get; set; }

        /// <summary>
        /// 精神分
        /// </summary>
        public int? SpiritScore { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public int TaskStatus { get; set; }

        /// <summary>
        /// 关联旧任务
        /// </summary>
        public Guid? TaskManagementIId { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }


		public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }
}
