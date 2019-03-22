﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace CWGL
{
    [AutoMapFrom(typeof(CWGLAdvanceChargeDetail))]
    public class CWGLAdvanceChargeDetailListOutputDto 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 预付款编号
        /// </summary>
        public Guid AdvanceChargeId { get; set; }

        /// <summary>
        /// 预付金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public int Mode { get; set; }

        /// <summary>
        /// 付款银行
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
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
