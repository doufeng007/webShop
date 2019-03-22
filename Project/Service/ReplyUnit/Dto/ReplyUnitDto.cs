using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class ReplyUnitDto
    {
        public int? Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int Sort { get; set; }
    }

    [AutoMapFrom(typeof(ReplyUnit))]
    public class GetReplyUnitForEditOutput : ReplyUnitDto
    {
    }

    [AutoMapFrom(typeof(ReplyUnit))]
    public class ReplyUnitList : ReplyUnitDto
    {
    }

    public class GetReplyUnitListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Sort";
            }
        }
    }
}