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
    [AutoMapFrom(typeof(B_PaymentPrepay))]
    public class B_PaymentPrepayListOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 单号
        /// </summary>
        public string Code { get; set; }

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
        /// 打款方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 打款金额
        /// </summary>
        public decimal PayAmout { get; set; }

        

        /// <summary>
        /// 打款时间
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public B_PrePayStatusEnum Status { get; set; }

        

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        

        /// <summary>
        /// 充值时间
        /// </summary>
        public DateTime CreationTime { get; set; }


    }


    public class B_PaymentPrepayListForWxOutputDto
    {

        public Guid Id { get; set; }



        /// <summary>
        /// 单号
        /// </summary>
        public string Code { get; set; }

        

        /// <summary>
        /// 打款方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 支付账户
        /// </summary>
        public string PayAcount { get; set; }

        /// <summary>
        /// 打款金额
        /// </summary>
        public decimal PayAmout { get; set; }



        /// <summary>
        /// 打款时间
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public B_PrePayStatusEnum Status { get; set; }

        /// <summary>
        /// 充值时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
