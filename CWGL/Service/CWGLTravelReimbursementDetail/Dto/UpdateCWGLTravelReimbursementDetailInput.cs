using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLTravelReimbursementDetail))]
    public class CreateOrUpdateCWGLTravelReimbursementDetailInput : CreateCWGLTravelReimbursementDetailInput
    {
        public Guid? Id { get; set; }
    }
}