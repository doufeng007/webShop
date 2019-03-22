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
    [AutoMapFrom(typeof(MeetingLlogistics))]
    public class MeetingLlogisticsOutputDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 事项
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 会议类型
        /// </summary>
        public Guid? MeetingTypeId { get; set; }

        public string MeetingTypeName { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        public long UserId { get; set; }


        public string UserName { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }



    }
}
