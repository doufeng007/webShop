using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XZGL
{
    public enum XZGLCarVariable
    {
        [Description("手动")] 手动 = 0,
        [Description("自动")] 自动 = 1,
        [Description("手自一体")] 手自一体 = 2
    }
    public enum XZGLCarStatus
    {
        [Description("可申请")] 可申请 = 0,
        [Description("停用")] 停用 = 1,
        [Description("借用中")] 借用中 = 2
    }
    public enum CarType
    {
        [Description("单位用车")] 单位用车 = 0,
        [Description("个人用车")] 个人用车 = 1,
        [Description("私车公用")] 私车公用 = 2
    }
    public enum CarRelationType
    {
        [Description("报销")] 报销 = 0,
        [Description("出差")] 出差 = 1
    }

}
