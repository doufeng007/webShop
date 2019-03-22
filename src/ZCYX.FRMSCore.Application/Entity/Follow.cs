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

namespace ZCYX.FRMSCore.Application
{
    [Serializable]
    [Table("Follow")]
    public class Follow : Entity<Guid>
    {
        #region 表字段
        
        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName(@"编号")]
        public Guid Id { get; set; }

        /// <summary>
        /// 关注类别
        /// </summary>
        [DisplayName(@"关注类别")]
        public FollowType BusinessType { get; set; }

        /// <summary>
        /// 关注编号
        /// </summary>
        [DisplayName(@"关注编号")]
        public Guid BusinessId { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        [DisplayName(@"参数")]
        public string Parameter { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName(@"创建时间")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        [DisplayName(@"创建人员")]
        public long CreatorUserId { get; set; }


        #endregion
    }
}