using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class GetWorkFlowUrlParameterInput
    {
        public Guid FlowId { get; set; }

        public Guid? StepId { get; set; }


        public Guid? TaskId { get; set; }

    }


}
