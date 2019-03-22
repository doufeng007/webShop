using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train.Enum;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public class GetCourseListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程类别
        /// </summary>
        public Guid? CourseType { get; set; }
        
        /// <summary>
        /// 是否已经完成
        /// </summary>
        public bool IsComplate { get; set; }
        
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
