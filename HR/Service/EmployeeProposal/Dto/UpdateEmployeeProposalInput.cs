using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace HR
{
    public class UpdateEmployeeProposalInput : EmployeeProposal
    {
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }
    }
}