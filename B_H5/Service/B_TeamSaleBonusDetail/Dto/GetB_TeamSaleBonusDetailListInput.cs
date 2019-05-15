using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_TeamSaleBonusDetailListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// Pid
        /// </summary>
        public Guid Pid { get; set; }

        /// <summary>
        /// MaxSale
        /// </summary>
        public decimal MaxSale { get; set; }

        /// <summary>
        /// MinSale
        /// </summary>
        public decimal MinSale { get; set; }

        /// <summary>
        /// Scale
        /// </summary>
        public decimal Scale { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
