using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using TaskGL.Enum;
using ZCYX.FRMSCore.Application;

namespace TaskGL
{
    [AutoMapTo(typeof(TaskManagement))]
    public class CreateTaskManagementInput : CreateWorkFlowInstance
    {
        #region 表字段

        /// <summary>
        /// DealWithUsers
        /// </summary>
        public string DealWithUsers { get; set; }

        /// <summary>
        /// 任务要求
        /// </summary>
        public string Requirement { get; set; }

        /// <summary>
        /// 签名文件
        /// </summary>
        public Guid? SignFileId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskManagementTypeEnum? Type { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [MaxLength(300,ErrorMessage = "任务名称长度必须小于300")]
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
        [Range(0, int.MaxValue,ErrorMessage="")]
        public TaskManagementStateEnum TaskStatus { get; set; }

        /// <summary>
        /// 关联旧任务
        /// </summary>
        public Guid? TaskManagementIId { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        [MaxLength(50,ErrorMessage = "任务编号长度必须小于50")]
        public string Number { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [MaxLength(500,ErrorMessage = "项目名称长度必须小于500")]
        public string ProjectName { get; set; }


        [Required]
        public long TaskCreateUserId { get; set; }


        /// <summary>
        /// 是否领导发起
        /// </summary>
        public bool IsCreateByleader { get; set; }



        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }

    /// <summary>
    /// 工作记录
    /// </summary>
    public class CreateOrUpdateTaskManagementRecordInput
    {
        public Guid? Id { get; set; }

        public Guid TaskManagementId { get; set; }

        public string Content { get; set; }
        public TaskManagementStateEnum TaskStatus { get; set; }


    }

    public class TaskManagementRecordOutput
    {
        public Guid? Id { get; set; }

        public Guid TaskManagementId { get; set; }

        public string Content { get; set; }
        public TaskManagementStateEnum TaskStatus {get;set;}
    }
}