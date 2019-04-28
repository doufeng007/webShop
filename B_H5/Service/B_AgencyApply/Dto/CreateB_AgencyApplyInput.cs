using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_AgencyApply))]
    public class CreateB_AgencyApplyInput 
    {
        #region 表字段
        /// <summary>
        /// 代理等级id
        /// </summary>
        public Guid AgencyLevelId { get; set; }

        /// <summary>
        /// 代理等级
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int AgencyLevel { get; set; }

        /// <summary>
        /// 邀请链接id
        /// </summary>
        public Guid? InviteUrlId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [MaxLength(50,ErrorMessage = "Tel长度必须小于50")]
        [Required(ErrorMessage = "必须填写Tel")]
        public string Tel { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [MaxLength(50,ErrorMessage = "VCode长度必须小于50")]
        [Required(ErrorMessage = "必须填写VCode")]
        public string VCode { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(50,ErrorMessage = "Pwd长度必须小于50")]
        [Required(ErrorMessage = "必须填写Pwd")]
        public string Pwd { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        [MaxLength(100,ErrorMessage = "WxId长度必须小于100")]
        public string WxId { get; set; }

        /// <summary>
        /// 国家 1 2 3 4 5
        /// </summary>
        [MaxLength(100,ErrorMessage = "Country长度必须小于100")]
        public string Country { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [MaxLength(50,ErrorMessage = "PNumber长度必须小于50")]
        [Required(ErrorMessage = "必须填写PNumber")]
        public string PNumber { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [MaxLength(100,ErrorMessage = "Provinces长度必须小于100")]
        public string Provinces { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [MaxLength(100,ErrorMessage = "City长度必须小于100")]
        public string City { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        [MaxLength(100,ErrorMessage = "County长度必须小于100")]
        public string County { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [MaxLength(200,ErrorMessage = "Address长度必须小于200")]
        public string Address { get; set; }

        /// <summary>
        /// 打款方式  1支付宝 2银行转账
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int PayType { get; set; }

        /// <summary>
        /// 打款金额
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal PayAmout { get; set; }

        /// <summary>
        /// 打款账户
        /// </summary>
        [MaxLength(50,ErrorMessage = "PayAcount长度必须小于50")]
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
        /// Status
        /// </summary>
        
        public B_AgencyApplyStatusEnum Status { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public GetAbpFilesOutput TouxiangFile { get; set; } = new GetAbpFilesOutput();


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