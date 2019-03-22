using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlowDictionary
{
    public class GetWorkFlowDictionariesInput : PagedResultRequestDto, IShouldNormalize
    {
        public Guid? RootId { get; set; }

        public void Normalize()
        {
           
        }

    }
}
