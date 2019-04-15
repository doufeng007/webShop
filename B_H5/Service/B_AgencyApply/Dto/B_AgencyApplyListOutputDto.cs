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
    [AutoMapFrom(typeof(B_AgencyApply))]
    public class B_AgencyApplyListOutputDto 
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
        /// 代理姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string AgencyLevelName { get; set; }

        public Guid AgencyLevelId { get; set; }

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
        /// 打款方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 打款金额
        /// </summary>
        public decimal PayAmout { get; set; }


        /// <summary>
        /// 打款日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public B_AgencyApplyStatusEnum Status { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string StatusTitle { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }


    public class B_AgencyApplyCount
    {
        /// <summary>
        /// 待审核人数
        /// </summary>
        public int WaitAuditCount { get; set; }

        /// <summary>
        /// 通过人数
        /// </summary>
        public int PassCount { get; set; }

        /// <summary>
        /// 未通过人数
        /// </summary>
        public int NoPassCount { get; set; }
    }
}
