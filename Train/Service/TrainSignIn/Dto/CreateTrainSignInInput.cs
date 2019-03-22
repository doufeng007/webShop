using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Train
{
    [AutoMapTo(typeof(TrainSignIn))]
    public class CreateTrainSignInInput 
    {
        #region 表字段

        /// <summary>
        /// 培训编号
        /// </summary>
        [Required]
        public Guid TrainId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required,Range(0,long.MaxValue,ErrorMessage ="错误的用户编号。")]
        public long UserId { get; set; }
		
        #endregion
    }
}