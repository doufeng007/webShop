using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace MeetingGL
{
    [AutoMapFrom(typeof(XZGLMeetingRoom))]
    public class XZGLMeetingRoomListOutputDto 
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
        /// 会议室使用数
        /// </summary>
        public int MeetingCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string MeetingRoomSizeType { get; set; }

        public int Number { get; set; }

        public string AdminUserName { get; set; }


        /// <summary>
        ///  0 空置 1 使用中
        /// </summary>
        public int BookingStatus { get; set; }

    
    }
}
