using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    [AutoMap(typeof(WorkFlowInstalledBase))]
    public class CreateWorkFlowInput : WorkFlowInstalledBase
    {

    }

    [AutoMap(typeof(WorkFlowInstalledBase))]
    public class EidtWorkFlowOutput : WorkFlowInstalledBase
    {

    }

    public class GetForEditByVersionNumInput
    {
        public Guid FlowId { get; set; }


        public int VersionNum { get; set; }
    }
}
