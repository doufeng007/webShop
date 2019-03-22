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
    [Table("UserTrainExperience")]
    public class UserTrainExperience : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 用户编号
        /// </summary>
        [DisplayName(@"用户编号")]
        public int UserId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        [DisplayName(@"课程编号")]
        public Guid CourseId { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        [DisplayName(@"课程类型")]
        public int CourseType { get; set; }

        /// <summary>
        /// 基本情况
        /// </summary>
        [DisplayName(@"基本情况")]
        public string BasicSituation { get; set; }

        /// <summary>
        /// 措施建议
        /// </summary>
        [DisplayName(@"措施建议")]
        public string Proposal { get; set; }

        /// <summary>
        /// 心得体会
        /// </summary>
        [DisplayName(@"心得体会")]
        public string Experience { get; set; }

        /// <summary>
        /// 心得体会附件
        /// </summary>
        [DisplayName(@"心得体会附件")]
        [MaxLength(200)]
        public string ExperienceFile { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [DisplayName(@"流程查阅人员")]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int? Status { get; set; }

        /// <summary>
        /// 抄送查阅人员
        /// </summary>
        [DisplayName(@"抄送查阅人员")]
        public string CopyForUsers { get; set; }


        #endregion
    }
}