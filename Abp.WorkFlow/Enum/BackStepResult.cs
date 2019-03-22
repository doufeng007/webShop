using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public enum BackStepResult
    {
        不能退回 = 1,
        当前任务不能退回 = 2,
        成功退回 = 3,
    }
}
