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
using Train.Enum;
using ZCYX.FRMSCore;

namespace Train
{
    [Serializable]
    [Table("UserTrainScoreRecord")]
    public class UserTrainScoreRecord : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 用户编号
        /// </summary>
        [DisplayName(@"用户编号")]
        public long UserId { get; set; }

        /// <summary>
        /// 积分来源类型
        /// </summary>
        [DisplayName(@"积分来源类型")]
        public TrainScoreFromType FromType { get; set; }

        /// <summary>
        /// 积分来源
        /// </summary>
        [DisplayName(@"积分来源")]
        public Guid FromId { get; set; }

        /// <summary>
        /// 业务id
        /// </summary>
        [DisplayName(@"业务id")]
        public Guid? BusinessId { get; set; }
        /// <summary>
        /// 分值
        /// </summary>
        [DisplayName(@"分值")]
        public int Score { get; set; }


        #endregion
    }
}