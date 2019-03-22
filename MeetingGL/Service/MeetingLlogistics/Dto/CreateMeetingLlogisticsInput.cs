using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MeetingGL
{
    [AutoMapTo(typeof(MeetingLlogistics))]
    public class CreateMeetingLlogisticsInput 
    {
        #region 表字段
        /// <summary>
        /// 事项
        /// </summary>
        [MaxLength(500,ErrorMessage = "事项长度必须小于500")]
        [Required(ErrorMessage = "必须填写事项")]
        public string Name { get; set; }

        /// <summary>
        /// 会议类型
        /// </summary>
        public Guid? MeetingTypeId { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        public long UserId { get; set; }


		
        #endregion
    }
}