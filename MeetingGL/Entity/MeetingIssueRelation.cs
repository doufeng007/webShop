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

namespace MeetingGL
{
    [Serializable]
    [Table("MeetingIssueRelation")]
    public class MeetingIssueRelation : Entity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 会议编号
        /// </summary>
        [DisplayName(@"会议编号")]
        public Guid MeetingId { get; set; }

        /// <summary>
        /// 议题编号
        /// </summary>
        [DisplayName(@"议题编号")]
        public Guid IssueId { get; set; }


        public string UserIds { get; set; }

        public MeetingIssueResultStatus Status { get; set; }


        #endregion
    }
}