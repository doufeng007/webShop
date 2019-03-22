using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Abp.WorkFlow.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class WorkFlowListOutIn
    {
        public WorkFlow workFlow { get; set; }
        public List<WorkFlowModel> workFlowModel { get; set; }
        public List<WorkFlowModelColumn> workFlowModelColumn { get; set; }
        public List<WorkFlowTemplate> workFlowTemplate { get; set; }
    }
    public class DeleteFlowFirstStepDataIn
    {
        public Guid FlowId { get; set; }
        public Guid StepId { get; set; }
        public string InstanceID { get; set; }
    }
}
