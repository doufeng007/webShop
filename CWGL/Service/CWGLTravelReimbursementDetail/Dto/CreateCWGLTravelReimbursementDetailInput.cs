using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLTravelReimbursementDetail))]
    public class CreateCWGLTravelReimbursementDetailInput
    {
        #region 表字段


        public DateTime BeginTime { get; set; }


        public DateTime EndTime { get; set; }


        /// <summary>
        /// 起止地点
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 交通工具
        /// </summary>
        public string Vehicle { get; set; }

        /// <summary>
        /// 出差天数
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 交通费
        /// </summary>
        public int? Fare { get; set; }

        /// <summary>
        /// 住宿费
        /// </summary>
        public int? Accommodation { get; set; }

        /// <summary>
        /// 其他费
        /// </summary>
        public int? Other { get; set; }


        #endregion
    }
}