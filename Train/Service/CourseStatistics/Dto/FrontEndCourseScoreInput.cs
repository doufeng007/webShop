using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public class FrontEndCourseScoreInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string UserName { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
