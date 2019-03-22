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
using Train.Enum;

namespace Train
{
    [Serializable]
    [Table("Train")]
    public class Train : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 培训名称
        /// </summary>
        [DisplayName(@"培训名称")]
        [MaxLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 培训类别
        /// </summary>
        [DisplayName(@"培训类别")]
        public Guid Type { get; set; }

        /// <summary>
        /// 培训地点
        /// </summary>
        [DisplayName(@"培训地点")]
        [MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 培训开始时间
        /// </summary>
        [DisplayName(@"培训开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 培训结束时间
        /// </summary>
        [DisplayName(@"培训结束时间")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        [DisplayName(@"发起人")]
        public long InitiatorId { get; set; }

        /// <summary>
        /// 培训简介
        /// </summary>
        [DisplayName(@"培训简介")]
        [MaxLength(500)]
        public string Introduction { get; set; }

        /// <summary>
        /// 参训人员
        /// </summary>
        [DisplayName(@"参训人员")]
        public string JoinUser { get; set; }

        /// <summary>
        /// 评论积分
        /// </summary>
        [DisplayName(@"评论积分")]
        public int? CommentScore { get; set; }

        /// <summary>
        /// 采纳心得积分
        /// </summary>
        [DisplayName(@"采纳心得积分")]
        public int? ExperienceScore { get; set; }

        /// <summary>
        /// 讲师
        /// </summary>
        [DisplayName(@"讲师")]
        public string LecturerUser { get; set; }

        /// <summary>
        /// 是否需要心得体会
        /// </summary>
        [DisplayName(@"是否需要心得体会")]
        public bool IsExperience { get; set; }

        /// <summary>
        /// 参与积分
        /// </summary>
        [DisplayName(@"参与积分")]
        public int? JoinScore { get; set; }

        /// <summary>
        /// 会议室编号
        /// </summary>
        [DisplayName(@"会议室编号")]
        public Guid? MeetingRoomId { get; set; }

        /// <summary>
        /// 交通
        /// </summary>
        [DisplayName(@"交通")]
        [MaxLength(200)]
        public string Traffic { get; set; }

        /// <summary>
        /// 就餐安排
        /// </summary>
        [DisplayName(@"就餐安排")]
        [MaxLength(200)]
        public string Eat { get; set; }

        /// <summary>
        /// 住宿
        /// </summary>
        [DisplayName(@"住宿")]
        [MaxLength(200)]
        public string Accommodation { get; set; }

        /// <summary>
        /// 投影系统
        /// </summary>
        [DisplayName(@"投影系统")]
        [MaxLength(200)]
        public string ProjectionSystem { get; set; }

        /// <summary>
        /// 白板
        /// </summary>
        [DisplayName(@"白板")]
        [MaxLength(200)]
        public string Whiteboard { get; set; }

        /// <summary>
        /// 音响系统
        /// </summary>
        [DisplayName(@"音响系统")]
        [MaxLength(200)]
        public string SoundSystem { get; set; }

        /// <summary>
        /// 音响系统
        /// </summary>
        [DisplayName(@"会议室")]
        [MaxLength(200)]
        public string MeetingRoom { get; set; }

        /// <summary>
        /// 抄送查阅人员
        /// </summary>
        [DisplayName(@"抄送查阅人员")]
        public string CopyForUsers { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [DisplayName(@"流程查阅人员")]
        public string DealWithUsers { get; set; }


        /// <summary>
        /// 人事接收抄送
        /// </summary>
        [DisplayName(@"人事接收抄送")]
        public string Personnel { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }

        /// <summary>
        /// 是否需要提醒
        /// </summary>
        [DisplayName(@"是否需要提醒")]
        public bool IsTips { get; set; }

        /// <summary>
        /// 提醒时间
        /// </summary>
        [DisplayName(@"提醒时间")]
        public int? TipsTime { get; set; }

        /// <summary>
        /// 提醒单位
        /// </summary>
        [DisplayName(@"提醒单位")]
        public TrainTipsType? TipsUnit { get; set; }

        /// <summary>
        /// 公文编号
        /// </summary>
        [DisplayName(@"公文编号")]
        public Guid? DocumentId { get; set; }
        #endregion
    }
}