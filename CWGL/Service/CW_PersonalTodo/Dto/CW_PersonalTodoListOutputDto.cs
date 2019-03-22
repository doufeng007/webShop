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
    [AutoMapFrom(typeof(CW_PersonalTodo))]
    public class CW_PersonalTodoListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// BusinessId
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// BusinessType
        /// </summary>
        public CW_PersonalType BusinessType { get; set; }


        public string BusinessType_Name { get; set; }

        /// <summary>
        /// CWType
        /// </summary>
        public RefundResultType CWType { get; set; }


        public string CWType_Name { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        public decimal Amout_Pay { get; set; }


        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Amout_Gather { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public CW_PersonalToStatus Status { get; set; }


        public string StatusTitle { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// FlowId
        /// </summary>
        public Guid? FlowId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        public Guid? WorkFlowModelId { get; set; }

        public bool HasSubmitPay { get; set; }

        public bool HasSubmitGather { get; set; }


        public string Title { get; set; }

       


    }
}
