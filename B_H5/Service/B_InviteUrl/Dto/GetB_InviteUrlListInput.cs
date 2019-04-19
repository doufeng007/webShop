﻿using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_InviteUrlListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// AgencyLevel
        /// </summary>
        public Guid AgencyLevel { get; set; }

        /// <summary>
        /// ValidityDataType
        /// </summary>
        public int ValidityDataType { get; set; }

        /// <summary>
        /// AvailableCount
        /// </summary>
        public int AvailableCount { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}