using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_PaymentPrepay))]
    public class CreateB_PaymentPrepayInput 
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        ///public long UserId { get; set; }


        /// <summary>
        /// 打款方式
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int PayType { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal PayAmout { get; set; }

        /// <summary>
        /// 开号银行
        /// </summary>
        [MaxLength(50,ErrorMessage = "BankName长度必须小于50")]
        public string BankName { get; set; }

        /// <summary>
        /// 银行户名
        /// </summary>
        [MaxLength(100,ErrorMessage = "BankUserName长度必须小于100")]
        public string BankUserName { get; set; }

        /// <summary>
        /// 打款账户  支付宝账号|银行账号
        /// </summary>
        [MaxLength(50,ErrorMessage = "PayAcount长度必须小于50")]
        [Required(ErrorMessage = "必须填写PayAcount")]
        public string PayAcount { get; set; }

        /// <summary>
        /// 打款日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public B_PrePayStatusEnum Status { get; set; }

        /// <summary>
        /// 不通过原因
        /// </summary>
        [MaxLength(500,ErrorMessage = "Reason长度必须小于500")]
        public string Reason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500,ErrorMessage = "Remark长度必须小于500")]
        public string Remark { get; set; }

        /// <summary>
        /// 审核备注
        /// </summary>
        [MaxLength(1000,ErrorMessage = "AuditRemark长度必须小于1000")]
        public string AuditRemark { get; set; }


        /// <summary>
        /// 打款凭证
        /// </summary>
        public List<GetAbpFilesOutput> CredentFiles { get; set; } = new List<GetAbpFilesOutput>();



        #endregion
    }
}