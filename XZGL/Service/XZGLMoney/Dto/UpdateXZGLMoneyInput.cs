using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using XZGL.Enums;

namespace XZGL
{
    public class UpdateXZGLMoneyInput : CreateXZGLMoneyInput
    {
		public Guid Id { get; set; }        
    }
    public class UpdateXZGLMoneyStatusInput 
    {
		public Guid Id { get; set; }        
		public XZGLPropertyStatus Status { get; set; }        
		public Guid? ReimbursementId { get; set; }        
    }
}