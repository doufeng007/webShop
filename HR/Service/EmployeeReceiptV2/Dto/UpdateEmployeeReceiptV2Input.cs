using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace HR
{
    public class UpdateEmployeeReceiptV2Input : CreateEmployeeReceiptV2Input
    {
		public Guid FlowId { get; set; }
       public Guid InStanceId { get; set; }
       public bool IsUpdateForChange { get; set; }        
    }
}