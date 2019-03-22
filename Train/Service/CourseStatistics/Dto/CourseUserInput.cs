using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public class CourseUserInput : PagedAndSortedInputDto, IShouldNormalize
    {

        public Guid CourseId { get; set; }

        /// <summary>
        /// 统计时间-年
        /// </summary>
        public int StatisticYear { get; set; }

        /// <summary>
        /// 统计时间-月
        /// </summary>
        public int StatisticMonth { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
