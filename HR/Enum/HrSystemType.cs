using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HR.Enum
{
    public enum HrSystemType
    {
        [Description("部门岗位制度")] 部门岗位制度 = 1,
        [Description("行政管理制度")] 行政管理制度 =2,
        [Description("人力资源制度")] 人力资源制度 = 3,
        [Description("财务管理制度")] 财务管理制度 =4,
        [Description("员工手册")] 员工手册 = 5
    }
}