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
using ZCYX.FRMSCore;
using Abp.AutoMapper;

namespace TaskGL
{
    [AutoMapFrom(typeof(TaskManagement))]
    public class TaskManagementLogDto
    {
        #region 表字段
        /// <summary>
        /// 任务要求
        /// </summary>
        [LogColumn(@"任务要求", IsLog = true)]
        public string Requirement { get; set; }

        /// <summary>
        /// 签名文件
        /// </summary>
        [LogColumn(@"签名文件", IsLog = true)]
        public Guid? SignFileId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [LogColumn(@"负责人", IsLog = true)]
        public string UserName { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        [LogColumn(@"任务类型", IsLog = true)]
        public string TypeName { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        [LogColumn(@"截止时间", IsLog = true)]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [LogColumn(@"任务名称", IsLog = true)]
        public string TaskName { get; set; }

        /// <summary>
        /// 是否紧急
        /// </summary>
        [LogColumn(@"是否紧急", IsLog = true)]
        public bool? IsUrgent { get; set; }

        /// <summary>
        /// 督办人
        /// </summary>
        [LogColumn(@"督办人", IsLog = true)]
        public string SuperintendentName { get; set; }

        /// <summary>
        /// 任务说明
        /// </summary>
        [LogColumn(@"任务说明", IsLog = true)]
        public string Explain { get; set; }

        /// <summary>
        /// 绩效量分
        /// </summary>
        [LogColumn(@"绩效量分", IsLog = true)]
        public int? PerformanceScore { get; set; }

        /// <summary>
        /// 精神分
        /// </summary>
        [LogColumn(@"精神分", IsLog = true)]
        public int? SpiritScore { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        [LogColumn(@"所属项目", IsLog = true)]
        public string ProjectName { get; set; }


        /// <summary>
        /// 任务编号
        /// </summary>
        [LogColumn(@"任务编号", IsLog = true)]
        public string Number { get; set; }


        #endregion
    }
}