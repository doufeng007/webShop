using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public class GetTrainSignInListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        [Required, Range(0, long.MaxValue, ErrorMessage = "错误的用户编号。")]
        public long UserId { get; set; }

        [Required]
        public Guid TrainId { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " SignInTime desc";
            }
        }
    }
    public class GetTrainSignInListByTimeInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime? SignInTime { get; set; }

        [Required]
        public Guid TrainId { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " SignInTime desc";
            }
        }
    }
}
