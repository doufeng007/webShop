using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GWGL
{
   

    public enum TaskTypeProperty
    {
        [Description("创建任务")] CreateTask = 0,
        [Description("分发")] Distribute = 1,
        [Description("办结归档")] Filing= 2
    }

    public enum CopyForTypeProperty
    {
        [Description("顺序")] Order = 0,
        [Description("同步")] Sync = 1
    }

}
