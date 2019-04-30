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
    [AutoMapFrom(typeof(B_OrderOut))]
    public class B_OrderOutListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 代理姓名
        /// </summary>
        public string UserName { get; set; }


        public int GoodsNumber { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amout { get; set; }


        /// <summary>
        /// 货款支付
        /// </summary>
        public decimal GoodsPayment { get; set; }

        /// <summary>
        /// 余额支付
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// DeliveryFee
        /// </summary>
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string AddressUserName { get; set; }


        /// <summary>
        /// 联系电话
        /// </summary>
        public string AddressTel { get; set; }


        /// <summary>
        /// 省份
        /// </summary>
        public string AddressProvinces { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string AddressCity { get; set; }

        public string Address { get; set; }



        /// <summary>
        /// 运单号
        /// </summary>
        public string CourierNum { get; set; }

        /// <summary>
        /// Stauts
        /// </summary>
        public OrderOutStauts Status { get; set; }




    }


    public class B_OrderOutMyListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amout { get; set; }

        public int GoodsNumber { get; set; }

        /// <summary>
        /// Stauts
        /// </summary>
        public OrderOutStauts Status { get; set; }


        /// <summary>
        /// 收件人
        /// </summary>
        public string AddressUserName { get; set; }



        /// <summary>
        /// 订单明细
        /// </summary>
        public List<B_OrderDetailOutputDto> GoodsList { get; set; } = new List<B_OrderDetailOutputDto>();
    }
}
