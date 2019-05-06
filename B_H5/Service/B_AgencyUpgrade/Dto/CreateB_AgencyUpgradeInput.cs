using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    
    public class CreateB_AgencyUpgradeInput 
    {
        #region 表字段
        public Guid ToAgencyLevelId { get; set; }


        /// <summary>
        /// 打款方式  1支付宝 2银行转账
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public PayAccountType PayType { get; set; }

        /// <summary>
        /// 打款金额
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "")]
        public decimal PayAmout { get; set; }

        /// <summary>
        /// 打款账户
        /// </summary>
        [MaxLength(50, ErrorMessage = "PayAcount长度必须小于50")]
        [Required(ErrorMessage = "必须填写PayAcount")]
        public string PayAcount { get; set; }

        /// <summary>
        /// 银行户名
        /// </summary>
        public string BankUserName { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        public string BankName { get; set; }


        /// <summary>
        /// 打款日期
        /// </summary>
        public DateTime PayDate { get; set; }


        /// <summary>
        /// 打款凭证
        /// </summary>
        public List<GetAbpFilesOutput> CredentFiles { get; set; } = new List<GetAbpFilesOutput>();


        /// <summary>
        /// 手持凭证
        /// </summary>
        public List<GetAbpFilesOutput> HandleCredentFiles { get; set; } = new List<GetAbpFilesOutput>();



        #endregion
    }
}