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
    public class GetUserCourseRecordListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 查询类型
        /// </summary>
        public MyCourseListType type { get; set; }
        public Guid? CourseType { get; set; }
        public string CourseName { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
