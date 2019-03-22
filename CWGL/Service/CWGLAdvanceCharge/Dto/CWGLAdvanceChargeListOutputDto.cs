using Abp.Application.Services.Dto;
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
    [AutoMapFrom(typeof(CWGLAdvanceCharge))]
    public class CWGLAdvanceChargeListOutputDto : BusinessWorkFlowListOutput
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
        /// 客户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 事由说明
        /// </summary>
        public string Cause { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal Money { get; set; }
        public decimal AdvanceChargeMoney { get; set; }

        /// <summary>
        /// 结清状态
        /// </summary>
        public int SettleState { get; set; }
        public string  SettleState_Name { get; set; }
        public long? CreatorUserId { get; set; }


    }
}
