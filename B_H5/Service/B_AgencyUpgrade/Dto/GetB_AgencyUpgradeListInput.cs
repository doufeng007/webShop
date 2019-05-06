using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_AgencyUpgradeListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// AgencyId
        /// </summary>
        public Guid AgencyId { get; set; }

        /// <summary>
        /// ToAgencyLevelId
        /// </summary>
        public Guid ToAgencyLevelId { get; set; }

        /// <summary>
        /// NeedPrePayAmout
        /// </summary>
        public decimal NeedPrePayAmout { get; set; }

        /// <summary>
        /// NeedDeposit
        /// </summary>
        public decimal NeedDeposit { get; set; }

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
