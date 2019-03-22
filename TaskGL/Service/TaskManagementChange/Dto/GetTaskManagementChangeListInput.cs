using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace TaskGL
{
    public class GetTaskManagementChangeListInput : PagedAndSortedInputDto, IShouldNormalize
    {

        /// <summary>
        /// 变更编号
        /// </summary>
        public Guid TaskManagementId { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
