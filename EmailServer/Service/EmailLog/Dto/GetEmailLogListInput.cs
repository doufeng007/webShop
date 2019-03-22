using Abp.Runtime.Validation;
using System;
using ZCYX.FRMSCore.Application.Dto;

namespace EmailServer
{
    public class GetEmailLogListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
