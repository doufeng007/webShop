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

namespace IMLib
{
    [Serializable]
    [Table("IM_InquiryResult")]
    public class IM_InquiryResult : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段


        /// <summary>
        /// 讨论组Id
        /// </summary>
        [DisplayName(@"讨论组Id")]
        [MaxLength(100)]
        public string IM_GroupId { get; set; }

        /// <summary>
        /// 意见征询Id
        /// </summary>
        [DisplayName(@"意见征询Id")]
        public Guid InquiryId { get; set; }

        /// <summary>
        /// 回复用户
        /// </summary>
        [DisplayName(@"回复用户")]
        public long? ReplyUserId { get; set; }


        public DateTime? ReplyDateTime { get; set; }


        /// <summary>
        /// 回复内容
        /// </summary>
        [DisplayName(@"回复内容")]
        public string ReplyContent { get; set; }


        public string MeaageType { get; set; }


        public Guid MessageId { get; set; }




        #endregion
    }
}