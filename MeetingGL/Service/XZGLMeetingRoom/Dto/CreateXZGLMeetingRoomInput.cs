using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MeetingGL
{
    [AutoMapTo(typeof(XZGLMeetingRoom))]
    public class CreateXZGLMeetingRoomInput 
    {
        #region 表字段
        /// <summary>
        /// 会议室名称
        /// </summary>
        [Required(ErrorMessage = "请输入会议室名称。")]
        [MaxLength(50, ErrorMessage = "会议室名称最大为50字符。")]
        public string Name { get; set; }

        /// <summary>
        /// 会议室位置
        /// </summary>
        [Required(ErrorMessage = "请输入会议室位置。")]
        [MaxLength(200, ErrorMessage = "会议室位置最大为200字符。")]
        public string Address { get; set; }

        public MeetingRoomSizeType MeetingRoomSizeType { get; set; }

        public int Number { get; set; }
        public bool IsEnable { get; set; }

        public long? UserId { get; set; }

        #endregion
    }
}