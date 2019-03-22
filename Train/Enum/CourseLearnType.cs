using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Train.Enum
{
    public enum CourseLearnType
    {
        [Description("选读")] Selected = 0,
        [Description("必读")] Must = 1,
        [Description("全员必读")] MustAll = 2
    }
}
