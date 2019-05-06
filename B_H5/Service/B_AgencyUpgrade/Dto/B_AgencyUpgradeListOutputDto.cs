using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    [AutoMapFrom(typeof(B_AgencyUpgrade))]
    public class B_AgencyUpgradeListOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// AgencyId
        /// </summary>
        public string AgencyName { get; set; }


        /// <summary>
        /// ToAgencyLevelId
        /// </summary>
        public Guid OldAgencyLevelId { get; set; }

        public string OldAgencyLevelName { get; set; }

        /// <summary>
        /// ToAgencyLevelId
        /// </summary>
        public Guid ToAgencyLevelId { get; set; }


        public string ToAgencyLevelName { get; set; }


        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }



        /// <summary>
        /// 身份证号码
        /// </summary>
        public string PNumber { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        public string WxId { get; set; }


        /// <summary>
        /// NeedPrePayAmout
        /// </summary>
        public decimal NeedPrePayAmout { get; set; }

        /// <summary>
        /// NeedDeposit
        /// </summary>
        public decimal NeedDeposit { get; set; }

        /// <summary>
        /// 打款方式
        /// </summary>
        public PayAccountType PayType { get; set; }

        /// <summary>
        /// 打款金额
        /// </summary>
        public decimal PayAmout { get; set; }


        /// <summary>
        /// 打款日期
        /// </summary>
        public DateTime PayDate { get; set; }


        /// <summary>
        /// Status
        /// </summary>
        public B_AgencyApplyStatusEnum Status { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
