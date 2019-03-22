using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace SearchAll
{
    public class GetImSearchListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 组编号
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
    public class GetImSearchInput {
        public string To { get; set; }
        public string SearchKey { get; set; }
    }
}
