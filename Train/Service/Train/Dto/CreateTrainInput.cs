using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Train.Enum;

namespace Train
{
    [AutoMapTo(typeof(Train))]
    public class CreateTrainInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 培训名称
        /// </summary>
        [Required,MaxLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 培训类别
        /// </summary>
        public Guid Type { get; set; }

        /// <summary>
        /// 培训地点
        /// </summary>
        [Required, MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 培训开始时间
        /// </summary>        
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 培训结束时间
        /// </summary>    
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        [Required,Range(0,long.MaxValue)]
        public long InitiatorId { get; set; }

        /// <summary>
        /// 培训简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 参训人员
        /// </summary>
        [Required]
        public string JoinUser { get; set; }

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
        [Required]
        public string LecturerUser { get; set; }

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
        public string SoundSystem { get; set; }

        /// <summary>
        /// 会议室
        /// </summary>
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

        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
}