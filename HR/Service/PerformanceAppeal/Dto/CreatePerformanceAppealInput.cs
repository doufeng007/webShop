using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace HR
{
    [AutoMapTo(typeof(PerformanceAppeal))]
    public class CreatePerformanceAppealInput : CreateWorkFlowInstance
    {
        public List<PerformanceAppealDetailInput> performanceAppealDetails { get; set; } = new List<PerformanceAppealDetailInput>();
    }
    public class PerformanceAppealDetailInput {
        public Guid PerformanceId { get; set; }
        public PerformanceType Type { get; set; }
        public string Content { get; set; }
    }
}