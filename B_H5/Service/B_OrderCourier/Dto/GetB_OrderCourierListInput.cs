using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_OrderCourierListInput : PagedAndSortedInputDto, IShouldNormalize
    {
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



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
