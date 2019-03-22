using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    public enum RelationType
    {
        [Description("请假")] AskForLeave = 0,
        [Description("出差")] Workout = 1,
    }
}
