﻿using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace HR.Service.EmployeeRequire.Dto
{
    public class EmployeeRequireSearchInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
        public long? CreateUserId { get; set; }

        public long? OrgId { get; set; }
        public List<int> Status { get; set; }
        public Guid FlowId { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
