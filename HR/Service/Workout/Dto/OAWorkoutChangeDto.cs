using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace HR
{
    [AutoMapFrom(typeof(OAWorkout))]
    public class OAWorkoutChangeDto
    {

        /// <summary>
        /// BeginTime
        /// </summary>
        [LogColumn("开始时间", IsLog = true)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [LogColumn("结束时间", IsLog = true)]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [LogColumn("工作内容", IsLog = true)]
        public string Reason { get; set; }



        [LogColumn("时长", IsLog = true)]
        public decimal? Hours { get; set; }


        [LogColumn("出发地点", IsLog = true)]
        public string FromPosition { get; set; }

        [LogColumn("出差地点", IsLog = true)]
        public string Destination { get; set; }


        [LogColumn("交通工具", IsLog = true)]
        public string TranType_Name { get; set; }

        [LogColumn("交通工具", IsLog = true)]
        public string RelationUserId_Name { get; set; }

        [LogColumn("是否用车", IsLog = true)]
        public string IsCar { get; set; }
    }
}