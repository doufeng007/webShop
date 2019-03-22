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
    [AutoMapFrom(typeof(MeetingRoomUseInfo))]
    public class MeetingRoomUseInfoOutputDto 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 会议室编号
        /// </summary>
        public Guid MeetingRoomId { get; set; }

        /// <summary>
        /// 业务id
        /// </summary>
        public Guid BusinessId { get; set; }

        public string BusinessName { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public MeetingRoomUseBusinessType BusinessType { get; set; }


        public string BusinessTypeName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        public string CreateUserName { get; set; }





    }
}
