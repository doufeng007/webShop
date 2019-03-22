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
    [AutoMapFrom(typeof(MeetingLlogistics))]
    public class MeetingLlogisticsListOutputDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 事项
        /// </summary>
        public string Name { get; set; }


        public Guid MeetingTypeId { get; set; }

        /// <summary>
        /// 会议类型
        /// </summary>
        public string MeetingTypeName { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        public string UserName { get; set; }


        public long UserId { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
