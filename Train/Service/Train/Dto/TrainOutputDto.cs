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
using Train.Enum;

namespace Train
{
    [AutoMapFrom(typeof(Train))]
    public class TrainOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 培训名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 培训类别
        /// </summary>
        public string TypeName { get; set; }
        public Guid Type { get; set; }

        /// <summary>
        /// 培训地点
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 培训开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 培训结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        public long InitiatorId { get; set; }
        public string InitiatorName { get; set; }

        /// <summary>
        /// 培训简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 参训人员
        /// </summary>
        public string JoinUser { get; set; }
        public string JoinUserName { get; set; }

        /// <summary>
        /// 评论积分
        /// </summary>
        public int? CommentScore { get; set; }

        /// <summary>
        /// 采纳心得积分
        /// </summary>
        public int? ExperienceScore { get; set; }

        /// <summary>
        /// 讲师
        /// </summary>
        public string LecturerUser { get; set; }
        public string LecturerUserName { get; set; }

        /// <summary>
        /// 是否需要心得体会
        /// </summary>
        public bool IsExperience { get; set; }

        /// <summary>
        /// 参与积分
        /// </summary>
        public int? JoinScore { get; set; }

        /// <summary>
        /// 会议室编号
        /// </summary>
        public Guid? MeetingRoomId { get; set; }

        /// <summary>
        /// 交通
        /// </summary>
        public string Traffic { get; set; }

        /// <summary>
        /// 就餐安排
        /// </summary>
        public string Eat { get; set; }

        /// <summary>
        /// 住宿
        /// </summary>
        public string Accommodation { get; set; }

        /// <summary>
        /// 投影系统
        /// </summary>
        public string ProjectionSystem { get; set; }

        /// <summary>
        /// 白板
        /// </summary>
        public string Whiteboard { get; set; }

        /// <summary>
        /// 音响系统
        /// </summary>
        /// </summary>
        public string SoundSystem { get; set; }
        public string MeetingRoom { get; set; }

        /// <summary>
        /// 是否需要提醒
        /// </summary>
        public bool IsTips { get; set; }

        /// <summary>
        /// 提醒时间
        /// </summary>
        public int? TipsTime { get; set; }

        /// <summary>
        /// 提醒单位
        /// </summary>
        public TrainTipsType? TipsUnit { get; set; }

        public Guid? DocumentId { get; set; }
        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }

    public class TrainCommentOutputDto {

        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        public List<CommentListOutput> CommentList { get; set; } = new List<CommentListOutput>();
    }
}
