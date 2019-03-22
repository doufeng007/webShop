using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingGL
{
    [AutoMapFrom(typeof(XZGLMeetingRoom))]
    public class XZGLMeetingRoomOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 会议室名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 会议室位置
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


        public MeetingRoomSizeType MeetingRoomSizeType { get; set; }

        public string MeetingRoomSizeTypeName { get; set; }

        public int Number { get; set; }

        public long? UserId { get; set; }


        public string AdminUserName { get; set; }

        public int BookingStatus { get; set; }



    }
}
