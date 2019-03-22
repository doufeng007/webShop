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
    public class GetEmployees_SignListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// SignType
        /// </summary>
        public int SignType { get; set; }

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
