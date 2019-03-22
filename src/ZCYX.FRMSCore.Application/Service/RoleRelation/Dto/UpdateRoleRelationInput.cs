using System;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace ZCYX.FRMSCore.Application
{
    public class UpdateRoleRelationInput : CreateRoleRelationInput
    {
		public Guid Id { get; set; }        
    }
}