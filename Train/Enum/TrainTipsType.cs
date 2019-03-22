using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Train.Enum
{
    public enum TrainTipsType
    {
        [Description("分钟")] Minute = 0,
        [Description("小时")] Hour = 1,
        [Description("天")] Day = 2
    }
}
