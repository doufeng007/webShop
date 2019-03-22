using Abp.AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapTo(typeof(Follow))]
    public class CreateFollowInput 
    {
        #region 表字段

        /// <summary>
        /// 关注类别
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public FollowType BusinessType { get; set; }

        /// <summary>
        /// 关注编号
        /// </summary>
        public Guid BusinessId { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }


		
        #endregion
    }
}