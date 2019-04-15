using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace B_H5
{
    public class AuditB_AgencyApplyInput 
    {
		public Guid Id { get; set; }    
        
        public string Reason { get; set; }

        public string Remark { get; set; }


        public bool IsPass { get; set; }
    }
}