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
    [AutoMapFrom(typeof(CWGLCredential))]
    public class CWGLCredentialOutputDto 
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
        public long UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 收款事由
        /// </summary>
        public string Cause { get; set; }
        public string ContractNum { get; set; }

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

        /// <summary>
        /// 电子资料
        /// </summary>
        public int? Nummber { get; set; }

        /// <summary>
        /// 关联编号
        /// </summary>
        public Guid? BusinessId { get; set; }

        public string FlowNumber { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public CredentialType BusinessType { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();

    }
}
