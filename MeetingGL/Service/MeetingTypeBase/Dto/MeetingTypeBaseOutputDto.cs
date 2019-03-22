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
    [AutoMapFrom(typeof(MeetingTypeBase))]
    public class MeetingTypeBaseOutputDto 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否启用回执
        /// </summary>
        public bool ReturnReceiptEnable { get; set; }

        /// <summary>
        /// 是否启用提醒
        /// </summary>
        public bool WarningEnable { get; set; }

        /// <summary>
        /// 会前提醒时间数
        /// </summary>
        public int? WarningDateNumber { get; set; }

        /// <summary>
        /// 会前提示时间类型
        /// </summary>
        public WraningDataTypeEnum? WraningDataType { get; set; }

        /// <summary>
        /// 会前提醒方式
        /// </summary>
        public WraingTypeEnum? WraingType { get; set; }

        /// <summary>
        /// 签到开始时间
        /// </summary>
        public int SignTime1 { get; set; }

        /// <summary>
        /// 签到结束时间
        /// </summary>
        public int SignTime2 { get; set; }

        /// <summary>
        /// 签到规则
        /// </summary>
        public int? SignType { get; set; }

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
