using System;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace ZCYX.FRMSCore.Application
{
    public class UpdateWorkListInput : CreateWorkListInput
    {
		public Guid Id { get; set; }        
    }
}