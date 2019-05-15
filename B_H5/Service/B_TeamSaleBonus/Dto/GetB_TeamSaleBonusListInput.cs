using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_TeamSaleBonusListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// EffectTime
        /// </summary>
        public DateTime EffectTime { get; set; }

        /// <summary>
        /// FailureTime
        /// </summary>
        public DateTime? FailureTime { get; set; }

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
