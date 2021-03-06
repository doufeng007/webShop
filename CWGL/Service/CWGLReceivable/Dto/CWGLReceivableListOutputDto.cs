﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using CWGL.Enums;

namespace CWGL
{
    [AutoMapFrom(typeof(CWGLReceivable))]
    public class CWGLReceivableListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 收款人
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public MoneyMode Mode { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        public string BankOpenName { get; set; }

        /// <summary>
        /// 事由摘要
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 纸质凭证
        /// </summary>
        public int? Nummber { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }



    }
}
