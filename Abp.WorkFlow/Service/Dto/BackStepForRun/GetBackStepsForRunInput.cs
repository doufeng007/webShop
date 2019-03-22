using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class GetBackStepsForRunInput
    {
        public Guid FlowId { get; set; }

        public Guid StepId { get; set; }

        public Guid GroupId { get; set; }

        public Guid TaskId { get; set; }



    }
}
