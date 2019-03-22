using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace HR
{
    [AutoMapTo(typeof(Daily))]
    public class CreateDailyInput 
    {
        #region 表字段

        /// <summary>
        /// Department
        /// </summary>
        [MaxLength(20,ErrorMessage = "Department长度必须小于20")]
        public string Department { get; set; }

        /// <summary>
        /// Personnel
        /// </summary>
        [MaxLength(20,ErrorMessage = "Personnel长度必须小于20")]
        public string Personnel { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// OverState
        /// </summary>
        [MaxLength(20,ErrorMessage = "OverState长度必须小于20")]
        public string OverState { get; set; }

        /// <summary>
        /// Note
        /// </summary>
        public string Note { get; set; }


		
        #endregion
    }
}