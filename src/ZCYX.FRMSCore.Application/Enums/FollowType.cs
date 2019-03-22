using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    public enum FollowType
    {
        [Description("公告")] 公告 = 0,
        [Description("物业管理")] 物业管理 = 1,
        [Description("单位信息")] 单位信息 = 2,
        [Description("费用管理")] 费用管理 = 3,
        [Description("任务管理")] 任务管理 = 4,
    }

}
