using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MeetingGL
{
    [AutoMapTo(typeof(MeetingPeriodRule))]
    public class CreateMeetingPeriodRuleInput 
    {
        #region 表字段


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
        [Range(0, int.MaxValue,ErrorMessage="")]
        public PeriodType PeriodType { get; set; }

        /// <summary>
        /// 重复参数1
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int PeriodNumber1 { get; set; }

        /// <summary>
        /// 重复参数2
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int PeriodNumber2 { get; set; }

        /// <summary>
        /// 重复参数3
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int PeriodNumber3 { get; set; }

        /// <summary>
        /// 提前创建时间类型
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public PreTimeType PreTimeType { get; set; }

        /// <summary>
        /// 提前创建时间数
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int PreTimeNum { get; set; }
		

        #endregion
    }


    public class UpdateMeetingPeriodRuleInput: CreateMeetingPeriodRuleInput
    {
        public Guid Id { get; set; }
    }

    public class PeriodClearInput {
        public Guid Id { get; set; }
        public Guid FlowId { get; set; }
    }
}