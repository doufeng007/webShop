using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public class GetTrainLogisticsListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 培训编号
        /// </summary>
        public Guid 培训编号 { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        public string 类型名 { get; set; }

        /// <summary>
        /// 类型值
        /// </summary>
        public string 类型值 { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
