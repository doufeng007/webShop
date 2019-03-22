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
    [Table("TrainUserExperience")]
    public class TrainUserExperience : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 用户编号
        /// </summary>
        [DisplayName(@"用户编号")]
        public long UserId { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        [DisplayName(@"类别")]
        public TrainExperienceType Type { get; set; }

        /// <summary>
        /// 流程编号
        /// </summary>
        [DisplayName(@"流程编号")]
        public Guid TrainId { get; set; }

        /// <summary>
        /// 心得体会
        /// </summary>
        [DisplayName(@"心得体会")]
        public string Experience { get; set; }

        /// <summary>
        /// 领导批示
        /// </summary>
        [DisplayName(@"领导批示")]
        public string Approval { get; set; }

        /// <summary>
        /// 是否采用
        /// </summary>
        [DisplayName(@"是否采用")]
        public bool IsUse { get; set; }


        #endregion
    }
}