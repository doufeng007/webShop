using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public class TrainScoreInput : PagedAndSortedInputDto, IShouldNormalize
    {

        /// <summary>
        /// 入职时间-起
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 入职时间-止
        /// </summary>
        public DateTime? EndTime { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
