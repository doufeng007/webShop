using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;

namespace MeetingGL
{
    [AutoMapFrom(typeof(MeetingPeriodRule))]
    public class MeetingPeriodRuleOutputDto 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 会议编号
        /// </summary>
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 会议开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 生效开始时间
        /// </summary>
        public DateTime ActiveStartTime { get; set; }

        /// <summary>
        /// 生效结束时间
        /// </summary>
        public DateTime ActiveEndTime { get; set; }

        /// <summary>
        /// 重复模式
        /// </summary>
        public PeriodType PeriodType { get; set; }

        /// <summary>
        /// 重复参数1
        /// </summary>
        public int PeriodNumber1 { get; set; }

        /// <summary>
        /// 重复参数2
        /// </summary>
        public int PeriodNumber2 { get; set; }

        /// <summary>
        /// 重复参数3
        /// </summary>
        public int PeriodNumber3 { get; set; }

        /// <summary>
        /// 提前创建时间类型
        /// </summary>
        public PreTimeType PreTimeType { get; set; }

        /// <summary>
        /// 提前创建时间数
        /// </summary>
        public int PreTimeNum { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


		
    }
}
