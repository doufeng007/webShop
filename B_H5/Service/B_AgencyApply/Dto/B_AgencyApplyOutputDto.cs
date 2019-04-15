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

namespace B_H5
{
    [AutoMapFrom(typeof(B_AgencyApply))]
    public class B_AgencyApplyOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 邀请代理
        /// </summary>
        public string InvitUserName { get; set; }

        /// <summary>
        /// 邀请代理电话
        /// </summary>
        public string InvitUserTel { get; set; }


        /// <summary>
        /// 邀请代理地址
        /// </summary>
        public string InvitUserAddress { get; set; }


        /// <summary>
        /// 代理编号
        /// </summary>
        public string AgencyLevelCode { get; set; }


        public Guid AgencyLevelId { get; set; }
        /// <summary>
        /// 代理等级
        /// </summary>
        public string AgencyLevelName { get; set; }


        /// <summary>
        /// 代理姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }


        /// <summary>
        /// 微信号
        /// </summary>
        public string WxId { get; set; }

        /// <summary>
        /// PNumber
        /// </summary>
        public string PNumber { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }

        

        /// <summary>
        /// Provinces
        /// </summary>
        public string Provinces { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// County
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// PayType
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// PayAmout
        /// </summary>
        public decimal PayAmout { get; set; }

        /// <summary>
        /// PayAcount
        /// </summary>
        public string PayAcount { get; set; }

        /// <summary>
        /// PayDate
        /// </summary>
        public DateTime PayDate { get; set; }


        /// <summary>
        /// 银行户名
        /// </summary>
        
        public string BankUserName { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        
        public string BankName { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public B_AgencyApplyStatusEnum Status { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

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



    }
}
