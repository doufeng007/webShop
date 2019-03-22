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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CWGL
{
    [AutoMapFrom(typeof(FinancialAccountingCertificate))]
    public class FinancialAccountingCertificateOutputDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType { get; set; }


        public string BusinessType_Name { get; set; }

        /// <summary>
        /// 业务编码
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 业务关联用户
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 用户所属部门
        /// </summary>
        public long? OrgId { get; set; }

        /// <summary>
        /// 记账人员
        /// </summary>
        public long? KeepUserId { get; set; }

        /// <summary>
        /// 审核人员
        /// </summary>
        public long? AuditUserId { get; set; }

        /// <summary>
        /// 出纳人员
        /// </summary>
        public long? CashierUserId { get; set; }

        /// <summary>
        /// 制单人员
        /// </summary>
        public long? MakeUserId { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [DisplayName(@"项目名称")]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        [DisplayName(@"地区")]
        [MaxLength(50)]
        public string Region { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [DisplayName(@"类别")]
        public Guid? Type { get; set; }
        public string  TypeName { get; set; } 

        /// <summary>
        /// 借方金额合计
        /// </summary>
        public decimal TotalDebitAmount { get; set; }

        /// <summary>
        /// 贷方金额合计
        /// </summary>
        public decimal TotalCreditAmount { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        public string ResultId { get; set; }

        public List<FACertificateDetailListOutputDto> Details { get; set; } = new List<FACertificateDetailListOutputDto>();


        public string CWCL_Name { get; set; }

        public string CWGL_Name { get; set; }



    }
}
