using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace XZGL
{
    public class GetXZGLCarBorrowListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 用车类型
        /// </summary>
        public CarType? CarType { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }

    public class GetXZGLCarBorrowByCarListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public Guid CarId { get; set; }
        public Guid FlowId { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }

    public class GetXZGLCarBorrowByWorkOutListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public Guid WorkOutId { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
