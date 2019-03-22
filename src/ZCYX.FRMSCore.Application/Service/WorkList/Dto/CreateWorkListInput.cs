using Abp.AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapTo(typeof(WorkList))]
    public class CreateWorkListInput 
    {
        #region 表字段

        /// <summary>
        /// 返回编号
        /// </summary>
        [MaxLength(32,ErrorMessage = "返回编号长度必须小于32")]
        public string BackId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


		
        #endregion
    }
}