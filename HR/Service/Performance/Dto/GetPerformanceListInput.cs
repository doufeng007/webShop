using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    public class GetPerformanceListInput : PagedAndSortedInputDto, IShouldNormalize
    {

        public long? UserId { get; set; }
        public bool GetMy { get; set; } = true;
        /// <summary>
        /// 类型
        /// </summary>
        public DateTime? Time { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
