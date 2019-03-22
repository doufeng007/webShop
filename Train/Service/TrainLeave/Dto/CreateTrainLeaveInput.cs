using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;

namespace Train
{
    [AutoMapTo(typeof(TrainLeave))]
    public class CreateTrainLeaveInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 培训编号
        /// </summary>
        public Guid TrainId { get; set; }

        /// <summary>
        /// 请假类型
        /// </summary>
        public Guid LevelType { get; set; }

        /// <summary>
        /// 请假事由
        /// </summary>
        public string Reason { get; set; }   

        /// <summary>
        /// 请假开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 请假结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 请假天数
        /// </summary>
        public decimal Day { get; set; }
        #endregion
    }
}