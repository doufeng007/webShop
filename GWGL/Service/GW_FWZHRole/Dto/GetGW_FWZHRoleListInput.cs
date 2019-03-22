using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace GWGL
{
    public class GetGW_FWZHRoleListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// StartIndex
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// AutoCoding
        /// </summary>
        public bool AutoCoding { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
