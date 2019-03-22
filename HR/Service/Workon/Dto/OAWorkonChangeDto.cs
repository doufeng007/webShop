using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace HR
{
    [AutoMap(typeof(OAWorkon))]
    public class OAWorkonChangeDto
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
    }
}