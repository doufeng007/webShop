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
using Abp.File;

namespace HR
{
    [Table("EmployeeProposal")]
    public class EmployeeProposal : FullAuditedEntity<Guid>, IMayHaveTenant
    {

        public string DealWithUsers { get; set; }
        public int? TenantId { get; set; }

        #region 表字段

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName(@"标题")]
        [MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName(@"类型")]
        public ProposalType Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName(@"内容")]
        public string Content { get; set; }

        /// <summary>
        /// 回复
        /// </summary>
        [DisplayName(@"回复")]
        public string Comment { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        [DisplayName("收件人")]
        public string ParticipateUser { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int? Status { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        [DisplayName("抄送人")]
        public string CopyForUsers { get; set; }

        /// <summary>
        /// 是否转入议题
        /// </summary>
        [DisplayName("是否转入议题")]
        public bool IsIssue { get; set; }
        
        /// <summary>
        /// 议题部门编号
        /// </summary>
        [DisplayName("议题部门编号")]
        public long? OrgId { get; set; }
        /// <summary>
        /// 议题项目编号
        /// </summary>
        [DisplayName("议题项目编号")]
        public Guid? SingleProjectId { get; set; }
        /// <summary>
        /// 议题汇报人
        /// </summary>
        [DisplayName("议题汇报人")]
        public long? IssueUserId { get; set; }
        /// <summary>
        /// 议题类型
        /// </summary>
        [DisplayName("议题类型")]
        public int? IssueType { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();

        #endregion
    }
}