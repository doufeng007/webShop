﻿using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetCode_AppraisalTypeListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }


        public int? ParentId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Sort";
            }
        }
    }
}
