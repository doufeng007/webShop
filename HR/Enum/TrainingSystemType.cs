using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HR.Enum
{
    public enum TrainingSystemType
    {
        [Description("公司")] Company = 0,
        [Description("岗位")] Post = 1
    }
}