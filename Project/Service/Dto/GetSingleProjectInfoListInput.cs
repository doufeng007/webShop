using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetSingleProjectInfoListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " SendTime desc";
            }
        }
    }
}
