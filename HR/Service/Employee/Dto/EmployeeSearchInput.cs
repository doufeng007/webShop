using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
   public  class EmployeeSearchInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 部门
        /// </summary>
        public long? OrgId { get; set; }
        /// <summary>
        /// 是否临时工
        /// </summary>
        public bool? IsTemp { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int? Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int? Month { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string SearchKey { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
