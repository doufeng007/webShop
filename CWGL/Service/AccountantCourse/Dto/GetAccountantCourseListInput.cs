using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace CWGL
{
    public class GetAccountantCourseListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 名称
        /// </summary>
        //public string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public Guid? Pid { get; set; }


        public bool IsAllChilds { get; set; } = false;

        /// <summary>
        /// parent_left
        /// </summary>
        //public int parent_left { get; set; }

        ///// <summary>
        ///// parent_right
        ///// </summary>
        //public int parent_right { get; set; }


        public bool IsOnlyRoot { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
