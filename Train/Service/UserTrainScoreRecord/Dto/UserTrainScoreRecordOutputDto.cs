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
    [AutoMapFrom(typeof(UserTrainScoreRecord))]
    public class UserTrainScoreRecordOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 积分来源类型
        /// </summary>
        public TrainScoreFromType FromType { get; set; }

        /// <summary>
        /// 积分来源
        /// </summary>
        public Guid FromId { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


		
    }
}
