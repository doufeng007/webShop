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
    /// <summary>
    /// 会议-列表
    /// </summary>
    [AutoMapFrom(typeof(XZGLMeeting))]
    public class XZGLMeetingListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 会议类型
        /// </summary>
        public string MeetingTypeName { get; set; }


        /// <summary>
        /// 会议室名称
        /// </summary>
        public string MeetingRoomName { get; set; }

        /// <summary>
        /// 会议开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 主持人
        /// </summary>
        public string ModeratorName { get; set; }

       

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }


    /// <summary>
    /// 周期会议-列表
    /// </summary>
    public class XZGLMeetingPeriodListOutputDto 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 会议名称
        /// </summary>
        public string Name { get; set; }

        
        /// <summary>
        /// 会议室地点
        /// </summary>
        public string MeetingRoomName { get; set; }

        /// <summary>
        /// 会议开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会议结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 重复模式
        /// </summary>
        public string PeriodName { get; set; }

        /// <summary>
        /// 生效开始时间
        /// </summary>
        public DateTime ActiveStartTime { get; set; }


        /// <summary>
        /// 生效结束时间
        /// </summary>
        public DateTime ActiveEndTime { get; set; }

        /// <summary>
        /// 参会人编号
        /// </summary>
        public string JoinPersonnel { get; set; }

        /// <summary>
        /// 参会人
        /// </summary>
        public string JoinPersonnelName { get; set; }


        public int Status { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string StatusTitle { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        /// <summary>
        /// 重复模式
        /// </summary>
        public PeriodType PeriodType { get; set; }

        /// <summary>
        /// 重复参数1
        /// </summary>
        public int PeriodNumber1 { get; set; }

        /// <summary>
        /// 重复参数2
        /// </summary>
        public int PeriodNumber2 { get; set; }

        /// <summary>
        /// 重复参数3
        /// </summary>
        public int PeriodNumber3 { get; set; }

        public Guid PeriodId { get; set; }
        public PeriodRuleStatus PeriodStatus { get; set; }
        public string PeriodStatusTitle { get; set; }


    }
}
