using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace XZGL
{
    public class GetXZGLCarListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public XZGLCarStatus? Status { get; set; }
        public bool? IsEnable { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
