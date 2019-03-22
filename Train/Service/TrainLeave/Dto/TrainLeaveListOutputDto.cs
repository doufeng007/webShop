using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace Train
{
    [AutoMapFrom(typeof(TrainLeave))]
    public class TrainLeaveListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 培训编号
        /// </summary>
        public Guid TrainId { get; set; }

        /// <summary>
        /// 请假类型
        /// </summary>
        public Guid? LevelType { get; set; }
        public string LevelTypeName { get; set; }

        /// <summary>
        /// 请假事由
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


        /// <summary>
        /// 请假开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 请假结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 请假天数
        /// </summary>
        public decimal? Day { get; set; }


    }
}
