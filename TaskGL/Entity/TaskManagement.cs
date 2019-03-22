using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using NPOI.HSSF.Model;
using TaskGL.Enum;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;

namespace TaskGL
{
    [Serializable]
    [Table("TaskManagement")]
    public class TaskManagement : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName(@"状态")]
        public int? Status { get; set; }

        /// <summary>
        /// DealWithUsers
        /// </summary>
        [DisplayName(@"DealWithUsers")]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// 任务要求
        /// </summary>
        [DisplayName(@"任务要求")]
        public string Requirement { get; set; }

        /// <summary>
        /// 签名文件
        /// </summary>
        [DisplayName(@"签名文件")]
        public Guid? SignFileId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [DisplayName(@"负责人")]
        public long? UserId { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        [DisplayName(@"任务类型")]
        public TaskManagementTypeEnum? Type { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        [DisplayName(@"截止时间")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [DisplayName(@"任务名称")]
        [MaxLength(300)]
        public string TaskName { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        [DisplayName(@"是否紧急")]
        public bool? IsUrgent { get; set; }

        /// <summary>
        /// 督办人
        /// </summary>
        [DisplayName(@"督办人")]
        public long? Superintendent { get; set; }

        /// <summary>
        /// 任务说明
        /// </summary>
        [DisplayName(@"任务说明")]
        public string Explain { get; set; }

        /// <summary>
        /// 绩效量分
        /// </summary>
        [DisplayName(@"绩效量分")]
        public int? PerformanceScore { get; set; }

        /// <summary>
        /// 精神分
        /// </summary>
        [DisplayName(@"精神分")]
        public int? SpiritScore { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        [DisplayName(@"所属项目")]
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [DisplayName(@"任务状态")]
        public TaskManagementStateEnum TaskStatus { get; set; }

        /// <summary>
        /// 关联旧任务
        /// </summary>
        [DisplayName(@"关联旧任务")]
        public Guid? TaskManagementIId { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        [DisplayName(@"任务编号")]
        [MaxLength(50)]
        public string Number { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DisplayName(@"项目名称")]
        [MaxLength(500)]
        public string ProjectName { get; set; }

        /// <summary>
        /// 任务发起人
        /// </summary>
        [DisplayName(@"任务发起人")]
        public long TaskCreateUserId { get; set; }


        /// <summary>
        /// 任务发起角色
        /// </summary>
        public CreateUserRoleTypeEnum CreateUserRoleType { get; set; }


        #endregion
    }
}