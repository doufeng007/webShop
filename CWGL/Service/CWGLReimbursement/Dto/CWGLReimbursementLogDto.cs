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
using CWGL.Enums;

namespace CWGL
{
	[AutoMapFrom(typeof(CWGLReimbursement))]
    public class CWGLReimbursementLogDto
    {
        #region 表字段         


        /// <summary>
        /// 金额
        /// </summary>
        [LogColumn(@"金额", IsLog = true)]
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        [LogColumn(@"收款方式", IsLog = true)]
        public string  Mode_Name { get; set; }

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

        /// <summary>
        /// 事由摘要
        /// </summary>
        [LogColumn(@"事由摘要", IsLog = true)]
        public string Note { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        [LogColumn(@"电子资料", IsLog = true)]
        public int? Nummber { get; set; }

        /// <summary>
        /// 关联备用金
        /// </summary>
        [LogColumn(@"关联备用金", IsLog = true)]
        public Guid? BorrowMoneyId { get; set; }



        #endregion
    }
}