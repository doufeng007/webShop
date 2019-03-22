using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace HR
{
    public class UpdatePerformanceAppealInput : CreateWorkFlowInstance
    {       
        public List<PerformanceAppealUpdateDetailInput> performanceAppealDetails { get; set; } = new List<PerformanceAppealUpdateDetailInput>();
    }

    public class PerformanceAppealUpdateDetailInput
    {
        public Guid Id { get; set; }
        public Guid PerformanceId { get; set; }
        public int Score { get; set; }
    }
}