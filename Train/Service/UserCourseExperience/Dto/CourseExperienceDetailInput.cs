using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public class CourseExperienceDetailInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public Guid CourseId { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
