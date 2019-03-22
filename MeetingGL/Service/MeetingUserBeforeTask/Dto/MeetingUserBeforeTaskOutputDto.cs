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
    [AutoMapFrom(typeof(MeetingUserBeforeTask))]
    public class MeetingUserBeforeTaskOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 会议编号
        /// </summary>
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public int TaskType { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        public XZGLMeetingOutputDto MeetingInfo { get; set; } = new XZGLMeetingOutputDto();


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }
}
