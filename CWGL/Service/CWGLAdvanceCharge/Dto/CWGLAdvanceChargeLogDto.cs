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
using Abp.AutoMapper;

namespace CWGL
{
	[AutoMapFrom(typeof(CWGLAdvanceCharge))]
    public class CWGLAdvanceChargeLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [LogColumn(@"流程查阅人员", IsLog = true)]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [LogColumn(@"Status", IsLog = true)]
        public int Status { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [LogColumn(@"客户名称", IsLog = true)]
        public string Name { get; set; }

        /// <summary>
        /// 事由说明
        /// </summary>
        [LogColumn(@"事由说明", IsLog = true)]
        public string Cause { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>
        [LogColumn(@"应付金额", IsLog = true)]
        public decimal Money { get; set; }

        /// <summary>
        /// 结清状态
        /// </summary>
        [LogColumn(@"结清状态", IsLog = true)]
        public int SettleState { get; set; }


        #endregion
    }
}