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

namespace CWGL
{
    /// <summary>
    /// 差旅费报销明细
    /// </summary>
    [Serializable]
    [Table("CWGLTravelReimbursementDetail")]
    public class CWGLTravelReimbursementDetail : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段


        public DateTime BeginTime { get; set; }


        public DateTime EndTime { get; set; }

        /// <summary>
        /// 起止地点
        /// </summary>
        [DisplayName(@"起止地点")]
        [MaxLength(128)]
        public string Address { get; set; }

        /// <summary>
        /// 交通工具
        /// </summary>
        [DisplayName(@"交通工具")]
        [MaxLength(10)]
        public string Vehicle { get; set; }

        /// <summary>
        /// 出差天数
        /// </summary>
        [DisplayName(@"出差天数")]
        public int? Day { get; set; }

        /// <summary>
        /// 交通费
        /// </summary>
        [DisplayName(@"交通费")]
        public decimal? Fare { get; set; }

        /// <summary>
        /// 住宿费
        /// </summary>
        [DisplayName(@"住宿费")]
        public decimal? Accommodation { get; set; }

        /// <summary>
        /// 其他费
        /// </summary>
        [DisplayName(@"其他费")]
        public decimal? Other { get; set; }

        /// <summary>
        /// 关联差旅费报销
        /// </summary>
        [DisplayName(@"关联差旅费报销")]
        public Guid TravelReimbursementId { get; set; }


        #endregion
    }
}