using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Train.Enum
{
    public enum CourseFavorState
    {
        [Description("赞")] Favor = 0,
        [Description("踩")] Diss = 1,
        [Description("取消")] None = 2
    }
}
