using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;
using Abp.AutoMapper;

namespace Train
{
	[AutoMapFrom(typeof(Train))]
    public class TrainLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 培训名称
        /// </summary>
        [LogColumn(@"培训名称", IsLog = true)]
        public string Title { get; set; }

        /// <summary>
        /// 培训类别
        /// </summary>
        [LogColumn(@"培训类别", IsLog = true)]
        public string Type { get; set; }

        /// <summary>
        /// 培训地点
        /// </summary>
        [LogColumn(@"培训地点", IsLog = true)]
        public string Address { get; set; }

        /// <summary>
        /// 培训开始时间
        /// </summary>
        [LogColumn(@"培训开始时间", IsLog = true)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 培训结束时间
        /// </summary>
        [LogColumn(@"培训结束时间", IsLog = true)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        [LogColumn(@"发起人", IsLog = true)]
        public string InitiatorName { get; set; }

        /// <summary>
        /// 培训简介
        /// </summary>
        [LogColumn(@"培训简介", IsLog = true)]
        public string Introduction { get; set; }

        /// <summary>
        /// 参训人员
        /// </summary>
        [LogColumn(@"参训人员", IsLog = true)]
        public string JoinUser { get; set; }

        /// <summary>
        /// 评论积分
        /// </summary>
        [LogColumn(@"评论积分", IsLog = true)]
        public int? CommentScore { get; set; }

        /// <summary>
        /// 采纳心得积分
        /// </summary>
        [LogColumn(@"采纳心得积分", IsLog = true)]
        public int? ExperienceScore { get; set; }

        /// <summary>
        /// 讲师
        /// </summary>
        [LogColumn(@"讲师", IsLog = true)]
        public string LecturerUser { get; set; }

        /// <summary>
        /// 是否需要心得体会
        /// </summary>
        [LogColumn(@"是否需要心得体会", IsLog = true)]
        public bool IsExperience { get; set; }

        /// <summary>
        /// 参与积分
        /// </summary>
        [LogColumn(@"参与积分", IsLog = true)]
        public int? JoinScore { get; set; }


        /// <summary>
        /// 交通
        /// </summary>
        [LogColumn(@"交通", IsLog = true)]
        public string Traffic { get; set; }

        /// <summary>
        /// 就餐安排
        /// </summary>
        [LogColumn(@"就餐安排", IsLog = true)]
        public string Eat { get; set; }

        /// <summary>
        /// 住宿
        /// </summary>
        [LogColumn(@"住宿", IsLog = true)]
        public string Accommodation { get; set; }

        /// <summary>
        /// 投影系统
        /// </summary>
        [LogColumn(@"投影系统", IsLog = true)]
        public string ProjectionSystem { get; set; }

        /// <summary>
        /// 白板
        /// </summary>
        [LogColumn(@"白板", IsLog = true)]
        public string Whiteboard { get; set; }

        /// <summary>
        /// 音响系统
        /// </summary>
        [LogColumn(@"音响系统", IsLog = true)]
        public string SoundSystem { get; set; }


        /// <summary>
        /// 会议室
        /// </summary>
        [LogColumn(@"会议室", IsLog = true)]
        public string MeetingRoom { get; set; }

        [LogColumn(@"是否需要提醒", IsLog = true)]
        public bool IsTips { get; set; }
        [LogColumn(@"提醒时间", IsLog = true)]
        public int? TipsTime { get; set; }
        [LogColumn(@"提醒单位", IsLog = true)]
        public string TipsUnit { get; set; }

        #endregion
    }
}