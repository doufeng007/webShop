using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Train.Enum;

namespace Train
{
    [AutoMapTo(typeof(UserTrainScoreRecord))]
    public class CreateUserTrainScoreRecordInput 
    {
        #region 表字段
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 积分来源类型
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public TrainScoreFromType FromType { get; set; }

        /// <summary>
        /// 积分来源
        /// </summary>
        public Guid FromId { get; set; }
        /// <summary>
        /// 业务id
        /// </summary>
        public Guid? BusinessId { get; set; }
        /// <summary>
        /// 分值
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Score { get; set; }


		
        #endregion
    }
}