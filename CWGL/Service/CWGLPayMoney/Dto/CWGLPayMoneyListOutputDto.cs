using Abp.Application.Services.Dto;
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
    [AutoMapFrom(typeof(CWGLPayMoney))]
    public class CWGLPayMoneyListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


        

        /// <summary>
        /// 借款人
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }


        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }



    }
}
