using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply
{
    public class GetSupplyListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }


        /// <summary>
        /// 按状态筛选
        /// </summary>
        public string Status { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }
    }

    
}
