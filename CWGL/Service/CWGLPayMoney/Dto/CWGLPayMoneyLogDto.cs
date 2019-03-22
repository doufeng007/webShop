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
	[AutoMapFrom(typeof(CWGLPayMoney))]
    public class CWGLPayMoneyLogDto
    {
        #region 表字段


        /// <summary>
        /// 借款人
        /// </summary>
        [LogColumn(@"借款人", IsLog = true)]
        public string UserName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [LogColumn(@"客户名称", IsLog = true)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [LogColumn(@"金额", IsLog = true)]
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        [LogColumn(@"收款方式", IsLog = true)]
        public string Mode_Name { get; set; }

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
        /// 合同编号
        /// </summary>
        [LogColumn(@"合同编号", IsLog = true)]
        public string ContractNum { get; set; }


        #endregion
    }
}