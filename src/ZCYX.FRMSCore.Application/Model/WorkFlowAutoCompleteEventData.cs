using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Model
{
    /// <summary>
    /// 流程自动完成事件
    /// </summary>
    public class WorkFlowAutoCompleteEventData: EventData
    {
        public Guid TaskId { get; set; }
    }
}
