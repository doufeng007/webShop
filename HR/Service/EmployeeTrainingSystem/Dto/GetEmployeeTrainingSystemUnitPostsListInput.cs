using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    public class GetEmployeeTrainingSystemUnitPostsListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 制度编号
        /// </summary>
        public Guid SysId { get; set; }

        /// <summary>
        /// 部门岗位编号
        /// </summary>
        public Guid PortsId { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
