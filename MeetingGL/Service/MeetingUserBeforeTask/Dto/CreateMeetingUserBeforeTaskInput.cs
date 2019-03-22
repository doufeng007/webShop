using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MeetingGL
{
    [AutoMapTo(typeof(MeetingUserBeforeTask))]
    public class CreateMeetingUserBeforeTaskInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 会议编号
        /// </summary>
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int TaskType { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [MaxLength(1000,ErrorMessage = "Remark长度必须小于1000")]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Status { get; set; }


		public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
}