using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public enum WorkFlowBusinessStatus
    {
        完成 = -1,
        驳回 = -2,
        整改中 = -3,
        已终止 = -4,
        作废 = -5,
    }
}
