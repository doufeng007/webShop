using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace HR
{
    [AutoMapFrom(typeof(EmployeeAskForLeave))]
    public class EmployeeAskForLeaveChangeDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid Id { get; set; }

        /// <summary>
        /// BeginTime
        /// </summary>
        [LogColumn("开始时间", IsLog = true)]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        [LogColumn("结束时间", IsLog = true)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [LogColumn("请假事由", IsLog = true)]
        public string Reason { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [LogColumn("备注", IsLog = true)]
        public string Remark { get; set; }


        [LogColumn("时长", IsLog = true)]
        public decimal? Hours { get; set; }

        /// <summary>
        /// 事项委托人
        /// </summary>
        [LogColumn("事项委托人", true)]
        public string RelationUserId_Name { get; set; }
    }
}
