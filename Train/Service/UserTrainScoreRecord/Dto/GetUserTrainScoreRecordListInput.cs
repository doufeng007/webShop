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
    public class GetUserTrainScoreRecordListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 积分来源类型
        /// </summary>
        public TrainScoreFromType FromType { get; set; }

        /// <summary>
        /// 积分来源
        /// </summary>
        public Guid FromId { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        public int Score { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
