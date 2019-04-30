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
    [AutoMapFrom(typeof(B_OrderCourier))]
    public class B_OrderCourierOutputDto 
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


		
    }
}
