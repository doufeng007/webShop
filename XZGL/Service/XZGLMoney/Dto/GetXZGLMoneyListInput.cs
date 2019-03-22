using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XZGL.Enums;
using ZCYX.FRMSCore.Application.Dto;

namespace XZGL
{
    public class GetXZGLMoneyListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 类型
        /// </summary>
        public Guid? Type { get; set; }
        public bool? IsFollow { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public XZGLPropertyStatus? Status { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
