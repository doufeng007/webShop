using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_AgencyLevelListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        public bool IsDefault { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
