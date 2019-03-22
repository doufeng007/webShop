using Abp.AutoMapper;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMap(typeof(CommendResume))]
    public  class CommendResumeOutput: InitWorkFlowOutput
    {
    }
}
