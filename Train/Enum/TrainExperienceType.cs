using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Train.Enum
{
    public enum TrainExperienceType
    {
        [Description("课程")] Course = 0,
        [Description("培训")] Train = 1,
    }
}
