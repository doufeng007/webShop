using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    public class GetEmployeeOtherInfoListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
    }
}
