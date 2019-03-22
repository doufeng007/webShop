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
    [Serializable]
    [Table("CWGLTravelReimbursement")]
    public class CWGLTravelReimbursement : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 报销人
        /// </summary>
        [DisplayName(@"报销人")]
        public long UserId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [DisplayName(@"部门")]
        public long OrgId { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DisplayName(@"金额")]
        public decimal Money { get; set; }

        /// <summary>
        /// 报销处理结果
        /// </summary>
        [DisplayName(@"报销处理结果")]
        public int ResultType { get; set; }

        /// <summary>
        /// 报销合计金额
        /// </summary>
        [DisplayName(@"报销合计金额")]
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 事由摘要
        /// </summary>
        [DisplayName(@"事由摘要")]
        public string Note { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        [DisplayName(@"电子资料")]
        public int? Nummber { get; set; }

        /// <summary>
        /// 关联备用金
        /// </summary>
        [DisplayName(@"关联备用金")]
        public Guid? BorrowMoneyId { get; set; }

        /// <summary>
        /// 关联出差
        /// </summary>
        [DisplayName(@"关联出差")]
        public Guid? WorkoutId { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [DisplayName(@"流程查阅人员")]
        [MaxLength(500)]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int Status { get; set; }


        [DisplayName(@"流程查阅人员排序状态")]
        [MaxLength(500)]
        public string DealWithUsersSort { get; set; }
        


        #endregion
    }
}