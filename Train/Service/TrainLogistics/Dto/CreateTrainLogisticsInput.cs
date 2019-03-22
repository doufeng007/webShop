using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System;

namespace Train
{
    [AutoMapTo(typeof(TrainLogistics))]
    public class CreateTrainLogisticsInput 
    {
        #region 表字段
        /// <summary>
        /// 培训编号
        /// </summary>
        public Guid 培训编号 { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        public string 类型名 { get; set; }

        /// <summary>
        /// 类型值
        /// </summary>
        public string 类型值 { get; set; }


		
        #endregion
    }
}