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
    [Table("MeetingIssue")]
    public class MeetingIssue : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段

        /// <summary>
        /// 事项
        /// </summary>
        [DisplayName(@"事项")]
        [MaxLength(500)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 汇报部门
        /// </summary>
        [DisplayName(@"汇报部门")]
        public long? OrgId { get; set; }

        /// <summary>
        /// 汇报人
        /// </summary>
        [DisplayName(@"汇报人")]
        public string UserId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName(@"内容")]
        public string Content { get; set; }


        public MeetingIssueStatus Status { get; set; }


        


        public Guid? RelationMeetingId { get; set; }


        /// <summary>
        /// 关联提案编号
        /// </summary>
        public Guid? RelationProposalId { get; set; }


        public IssueType IssueType { get; set; }

        public Guid? SingleProjectId { get; set; }

        /// <summary>
        /// 关联项目负责人
        /// </summary>
        public long? ProjectLeaderId { get; set; }


        


        #endregion
    }
}