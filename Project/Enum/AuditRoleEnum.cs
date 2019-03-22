using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Project
{
    public enum AuditRoleEnum
    {
        评审组 = -1,
        项目负责人 = 1,
        工程评审 = 2,
        财务评审 = 5,
        复核人 = 6,
        复核人二 = 7,
        复核人三 = 8,
        联系人一 = 9,
        联系人二 = 10,
        财务初审 = 11,
        汇总人员 = 12,

    }

    public enum FinishResultTypeEnum
    {
        评审结果 = 1,
        汇总结果 = 2,

    }
    public enum PersonOnChargeTypeEnum
    {
        [Description("普通")] 普通 = 0,
        [Description("待汇总")] 待汇总 = 1,
        [Description("已汇总")] 已汇总 = 3,
        [Description("负责人")] 负责人 = 2
    }



    public enum ProjectStopTypeEnum
    {
        [Description("未停滞")]
        NoStop = 0,
        [Description("停滞申请")]
        AapplyStop = 1,
        [Description("待解除申请")]
        WaitRelieve = 2,
        [Description("解除申请")]
        Relieve = 3,
    }
}
