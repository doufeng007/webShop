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
	[AutoMapFrom(typeof(CWGLRepayment))]
    public class CWGLRepaymentLogDto
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
        /// 金额
        /// </summary>
        [LogColumn(@"金额", IsLog = true)]
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        [LogColumn(@"收款方式", IsLog = true)]
        public string Mode { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [LogColumn(@"银行名称", IsLog = true)]
        public string BankName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        [LogColumn(@"卡号", IsLog = true)]
        public string CardNumber { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        [LogColumn(@"开户行名称", IsLog = true)]
        public string BankOpenName { get; set; }


        #endregion
    }
}