using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public class GetTrainListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 培训名称
        /// </summary>
        public Guid? Type { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
    public class GetTrainExperienceListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
    public class GetTrainMyListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 培训名称
        /// </summary>
        public int? State { get; set; }
        public Guid? Type { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
