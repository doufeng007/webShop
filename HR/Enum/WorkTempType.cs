using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HR.Enum
{
    public enum WorkTempType
    {
        [Description("请假")] 请假 = 0,
        [Description("出差")] 出差 = 1,
        [Description("加班")] 加班 = 2
    }
}