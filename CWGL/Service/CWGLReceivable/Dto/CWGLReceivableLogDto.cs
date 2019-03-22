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
	[AutoMapFrom(typeof(CWGLReceivable))]
    public class CWGLReceivableLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 收款人
        /// </summary>
        [LogColumn(@"收款人", IsLog = true)]
        public string UserName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [LogColumn(@"客户名称", IsLog = true)]
        public string Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [LogColumn(@"金额", IsLog = true)]
        public decimal Money { get; set; }

        /// <summary>
        /// 付款方式
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

        /// <summary>
        /// 事由摘要
        /// </summary>
        [LogColumn(@"事由摘要", IsLog = true)]
        public string Note { get; set; }

        /// <summary>
        /// 纸质凭证
        /// </summary>
        [LogColumn(@"纸质凭证", IsLog = true)]
        public int? Nummber { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        #endregion
    }
}