using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XZGL.Enums
{
    public enum XZGLPropertyStatus
    {
        [Description("未缴")] 未缴 = 0,
        [Description("已缴")] 已缴 = 1,
        [Description("报销中")] 报销中 = 2,
        [Description("已报销")] 已报销 = 3,
    }
}
