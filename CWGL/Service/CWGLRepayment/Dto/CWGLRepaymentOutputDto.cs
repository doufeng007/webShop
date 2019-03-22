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

namespace CWGL
{
    [AutoMapFrom(typeof(CWGLRepayment))]
    public class CWGLRepaymentOutputDto : WorkFlowTaskCommentResult
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
        public int Status { get; set; }

        /// <summary>
        /// 借款编号
        /// </summary>
        public Guid BorrowMoneyId { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public int Mode { get; set; }

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


		public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }
}
