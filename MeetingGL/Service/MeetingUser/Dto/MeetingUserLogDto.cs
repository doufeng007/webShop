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

namespace MeetingGL
{
	[AutoMapFrom(typeof(MeetingUser))]
    public class MeetingUserLogDto
    {
        #region 表字段
                /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 会议编号
        /// </summary>
        [LogColumn(@"会议编号", IsLog = true)]
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 会议人员角色
        /// </summary>
        [LogColumn(@"会议人员角色", IsLog = true)]
        public int MeetingUserRole { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        [LogColumn(@"人员", IsLog = true)]
        public long UserId { get; set; }

        /// <summary>
        /// 回执状态
        /// </summary>
        [LogColumn(@"回执状态", IsLog = true)]
        public int ReturnReceiptStatus { get; set; }

        /// <summary>
        /// 回执时间
        /// </summary>
        [LogColumn(@"回执时间", IsLog = true)]
        public DateTime? ConfirmData { get; set; }

        /// <summary>
        /// 请假备注
        /// </summary>
        [LogColumn(@"请假备注", IsLog = true)]
        public string AskForLeaveRemark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [LogColumn(@"状态", IsLog = true)]
        public int Status { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        #endregion
    }
}