using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Abp.WorkFlow.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class WorkFlowModelNew
    {
        public Guid OldGuid { get; set; }
        public Guid NewGuid { get; set; }
    }
}
