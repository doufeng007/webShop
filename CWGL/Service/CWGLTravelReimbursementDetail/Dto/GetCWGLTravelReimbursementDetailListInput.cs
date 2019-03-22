using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace CWGL
{
    public class GetCWGLTravelReimbursementDetailListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 起月
        /// </summary>
        public int StartMonth { get; set; }

        /// <summary>
        /// 起日
        /// </summary>
        public int StartDay { get; set; }

        /// <summary>
        /// 止月
        /// </summary>
        public int EndMonth { get; set; }

        /// <summary>
        /// 止日
        /// </summary>
        public int EndDay { get; set; }

        /// <summary>
        /// 起止地点
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 交通工具
        /// </summary>
        public string Vehicle { get; set; }

        /// <summary>
        /// 出差天数
        /// </summary>
        public int? Day { get; set; }

        /// <summary>
        /// 交通费
        /// </summary>
        public int? Fare { get; set; }

        /// <summary>
        /// 住宿费
        /// </summary>
        public int? Accommodation { get; set; }

        /// <summary>
        /// 其他费
        /// </summary>
        public int? Other { get; set; }

        /// <summary>
        /// 关联差旅费报销
        /// </summary>
        public Guid TravelReimbursementId { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
