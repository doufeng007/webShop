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
    [AutoMapFrom(typeof(B_OrderCourier))]
    public class B_OrderCourierListOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// OrderId
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// CourierNum
        /// </summary>
        public string CourierNum { get; set; }

        /// <summary>
        /// CourierName
        /// </summary>
        public string CourierName { get; set; }

        /// <summary>
        /// DeliveryFee
        /// </summary>
        public decimal DeliveryFee { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
