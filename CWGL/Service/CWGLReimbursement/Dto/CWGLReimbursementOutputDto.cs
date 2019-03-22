using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;
using CWGL.Enums;

namespace CWGL
{
    [AutoMapFrom(typeof(CWGLReimbursement))]
    public class CWGLReimbursementOutputDto : WorkFlowTaskCommentResult
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
        /// 流程查阅人员
        /// </summary>
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 报销人
        /// </summary>
        public long UserId { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public long OrgId { get; set; }

        public string DepartmentName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 收款方式
        /// </summary>
        public MoneyMode Mode { get; set; }
        public string ModeName { get; set; }

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
        /// 电子资料
        /// </summary>
        public int? Nummber { get; set; }

        /// <summary>
        /// 关联备用金
        /// </summary>
        public Guid BorrowMoneyId { get; set; }

        public decimal BorrowMoney { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }
}
