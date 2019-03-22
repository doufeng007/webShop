using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using CWGL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Extensions;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLCredential))]
    public class CreateCWGLCredentialInput 
    {
        #region 表字段
        /// <summary>
        /// 客户名称
        /// </summary>
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 收款事由
        /// </summary>
        [MaxLength(200)]
        [Required]
        public string Cause { get; set; }


        [MaxLength(30)]
        public string ContractNum { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Required]
        [Money]
        public decimal Money { get; set; }

        public bool IsPay { get; set; }
        /// <summary>
        /// 收款方式
        /// </summary>
        [Required]
        public MoneyMode Mode { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [MaxLength(20)]
        public string BankName { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        [MaxLength(30)]
        public string BankOpenName { get; set; }

        [MaxLength(30)]
        public string FlowNumber { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        public int? Nummber { get; set; }

        /// <summary>
        /// 关联编号
        /// </summary>
        public Guid? BusinessId { get; set; }
        public DateTime?  CreationTime{ get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public CredentialType BusinessType { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();

        #endregion
    }
}