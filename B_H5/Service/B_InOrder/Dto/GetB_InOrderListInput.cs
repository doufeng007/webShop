using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
   
    public class GetB_InOrderListInput : PagedAndSortedInputDto, IShouldNormalize
    {

        /// <summary>
        /// 状态 为空为全部
        /// </summary>
        public InOrderStatusEnum? Status { get; set; }

        public DateTime? StartDate { get; set; }


        public DateTime? EndDate { get; set; }


        public long? UserId { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
