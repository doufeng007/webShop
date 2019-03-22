using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;

namespace GWGL
{
    public class GetGW_DocumentTypeListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public GW_EmployeesSignStatusEnmu? Status { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
