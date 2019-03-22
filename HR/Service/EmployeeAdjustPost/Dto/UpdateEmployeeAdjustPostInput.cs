using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Service.EmployeeAdjustPost.Dto
{
    public class UpdateEmployeeAdjustPostInput
    {
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }
        public Guid Id { get; set; }
        public string WorkflowAdjsutDepId { get; set; }
        public string Remark { get; set; }
        public string AdjustDepId { get; set; }
        public Guid AdjustPostId { get; set; }
    }
}
