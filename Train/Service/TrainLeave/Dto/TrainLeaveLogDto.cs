using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;
using Abp.AutoMapper;

namespace Train
{
	[AutoMapFrom(typeof(TrainLeave))]
    public class TrainLeaveLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 请假类型
        /// </summary>
        [LogColumn(@"请假类型", IsLog = true)]
        public string LevelTypeName { get; set; }

        /// <summary>
        /// 请假事由
        /// </summary>
        [LogColumn(@"请假事由", IsLog = true)]
        public string Reason { get; set; }

        /// <summary>
        /// 请假开始时间
        /// </summary>
        [LogColumn(@"请假开始时间", IsLog = true)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 请假结束时间
        /// </summary>
        [LogColumn(@"请假结束时间", IsLog = true)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 请假天数
        /// </summary>
        [LogColumn(@"请假天数", IsLog = true)]
        public decimal Day { get; set; }


        #endregion
    }
}