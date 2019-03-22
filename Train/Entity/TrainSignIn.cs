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

namespace Train
{
    [Serializable]
    [Table("TrainSignIn")]
    public class TrainSignIn : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// 培训编号
        /// </summary>
        [DisplayName(@"培训编号")]
        public Guid TrainId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [DisplayName(@"用户编号")]
        public long UserId { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>
        [DisplayName(@"签到时间")]
        public DateTime SignInTime { get; set; }

        /// <summary>
        /// 签退时间
        /// </summary>
        [DisplayName(@"签退时间")]
        public DateTime? SignOutTime { get; set; }

        /// <summary>
        /// 签到ip
        /// </summary>
        [DisplayName(@"签到ip")]
        [MaxLength(50)]
        public string SignInIp { get; set; }

        /// <summary>
        /// 签退ip
        /// </summary>
        [DisplayName(@"签退ip")]
        [MaxLength(50)]
        public string SignOutIp { get; set; }


        #endregion
    }
}