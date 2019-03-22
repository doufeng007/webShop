using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace HR
{
    [AutoMapFrom(typeof(WorkRecord))]
    public class WorkRecordChangeDto
    {
        [LogColumn("工作内容", IsLog = false)]
        public string Content { get; set; }


        [LogColumn("开始时间", IsLog = false)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [LogColumn("结束时间", IsLog = false)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Remuneration
        /// </summary>
        [LogColumn("报酬", IsLog = false)]
        public decimal Remuneration { get; set; }

        /// <summary>
        /// 数字化绩效
        /// </summary>
        [LogColumn("数字化绩效", IsLog = false)]
        public decimal? DataPerformance { get; set; }
        /// <summary>
        /// 非数字化绩效
        /// </summary>
        [LogColumn("数字化绩效", IsLog = false)]
        public decimal? NoDataPerformance { get; set; }
    }
}
