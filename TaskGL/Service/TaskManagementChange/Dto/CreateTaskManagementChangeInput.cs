using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using TaskGL.Enum;

namespace TaskGL
{
    [AutoMapTo(typeof(TaskManagementChange))]
    public class CreateTaskManagementChangeInput 
    {
        #region 表字段
        /// <summary>
        /// 变更类型
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public TaskManagementChangeTypeEnum Type { get; set; }

        /// <summary>
        /// 变更编号
        /// </summary>
        public Guid TaskManagementId { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        [Required(ErrorMessage = "必须填写原因")]
        public TaskManagementReasonEnum Reason { get; set; }

        /// <summary>
        /// 评估
        /// </summary>
        public string Assessment { get; set; }

        /// <summary>
        /// 要求
        /// </summary>
        public string Requirement { get; set; }


        /// <summary>
        /// 绩效分
        /// </summary>
        public int? PerformanceScore { get; set; }

        /// <summary>
        /// 精神分
        /// </summary>
        public int? SpiritScore { get; set; }

        public long CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }
        #endregion
    }
}