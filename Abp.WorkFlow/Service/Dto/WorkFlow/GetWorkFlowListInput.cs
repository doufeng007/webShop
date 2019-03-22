using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class GetWorkFlowListInput : PagedResultRequestDto, IShouldNormalize
    {
        public Guid? TypeId { get; set; }


        public string SearchKey { get; set; }


        /// <summary>
        /// ,分隔的int
        /// </summary>
        public string Status { get; set; }

        public void Normalize()
        {

        }

        

    }
}
