using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class GetWorkFlowInstanceStatusOutput
    {
        public int Status { get; set; }

        public string StatusSummary { get; set; }
    }
}
