using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MeetingGL
{
    [AutoMapTo(typeof(MeetingTypeBase))]
    public class CreateMeetingTypeBaseInput 
    {
        #region 表字段
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(500,ErrorMessage = "名称长度必须小于500")]
        [Required(ErrorMessage = "必须填写名称")]
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
        public string SignType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Status { get; set; }


		
        #endregion
    }
}