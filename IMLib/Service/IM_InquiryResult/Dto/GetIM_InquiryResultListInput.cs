using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace IMLib
{
    public class GetIM_InquiryResultListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 讨论组Id
        /// </summary>
        public string IM_GroupId { get; set; }

        /// <summary>
        /// 意见征询Id
        /// </summary>
        public Guid InquiryId { get; set; }

        /// <summary>
        /// 回复用户
        /// </summary>
        public long ReplyUserId { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
