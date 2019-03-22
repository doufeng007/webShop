﻿using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace XZGL
{
    public class GetXZGLCompanyListInput : PagedAndSortedInputDto, IShouldNormalize
    {

        /// <summary>
        /// 类型
        /// </summary>
        public Guid? Type { get; set; }
        public int? Status { get; set; }
        public bool? IsFollow { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
