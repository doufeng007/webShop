using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace HR
{
    [AutoMapTo(typeof(Performance))]
    public class CreatePerformanceInput 
    {
        #region 表字段
        /// <summary>
        /// 用户
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 事项
        /// </summary>
        public string Matter { get; set; }

        /// <summary>
        /// 记录
        /// </summary>
        [Required(ErrorMessage = "必须填写记录")]
        public string Record { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public PerformanceType Type { get; set; }


		
        #endregion
    }
}